using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossABehaviour : MonoBehaviour
{
    [Header("�e���p�^�[��")]
    [SerializeField]
    GameObject[] _particles;
    [Header("���S���̉��i�`���[�W���j")]
    [SerializeField]
    AudioClip _deathChargeSE;
    [Header("���S���̉��i�����j")]
    [SerializeField]
    AudioClip _deathExplodeSE;
    [Header("���S���̃G�t�F�N�g�i�����j")]
    [SerializeField]
    GameObject _explodePrefab;

    Dictionary<string, GameObject> _particlesDict;
    Transform[] _pos;
    /// <summary>�U���p�^�[��������B�����O�ɃV�[���ړ������ۂ�Kill�ł���悤�ɕۑ�</summary>
    Sequence _seq;
    /// <summary>�{�X�̌����ڕ����B</summary>
    GameObject _bossCube;
    /// <summary>�J�n���̈ʒu</summary>
    Vector2 _startPos;
    /// <summary>�e���𔭐�������ʒu</summary>
    Transform _particleTr;
    /// <summary>SE��炷���߂�AudioSource���擾</summary>
    AudioSource _seAus;
    private void Start()
    {
        _particlesDict = _particles.ToDictionary(n => n.name, n => n);
        _startPos = this.transform.position;
        _particleTr = this.transform.Find("ParticlePosition").transform;
        _bossCube = this.transform.Find("BossCube").gameObject;
        _pos = GameObject.Find("Positions").transform.GetComponentsInChildren<Transform>();
        _seAus = GetComponent<AudioSource>();
        _seAus.volume *= PlayerPrefs.GetFloat("SEVolume");
        _bossCube.transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear);

        WanderingMove();
    }
    private void WanderingMove()
    {
        this.transform.DOMove(new Vector2(-_startPos.x, _startPos.y), 3).
             SetLoops(2, LoopType.Yoyo).
             SetEase(Ease.InOutQuad).
             OnComplete(() =>
             {
                 _bossCube.transform.DOPause();
                 Attack();
             });
    }
    private void Attack()
    {
        int index = Random.Range(0, 2);
        if (index == 0)
        {
            AttackPatternOne();
        }
        else if (index == 1)
        {
            AttackPatternTwo();
        }
        else
        {
            Debug.Log("Attack failed");
        }
    }
    private void AttackPatternOne()
    {
        float duration = 4.5f;
        ParticleSystem ps = Instantiate(_particlesDict["ShotgunShot"], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = ps.emission;
        var main = ps.main;
        ps.Stop();
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 720, 0), 0.5f, RotateMode.FastBeyond360).
            OnStart(() => this.transform.DOMove(_pos[1].position, 0.5f))
            );
        _seq.Append(
            this.transform.DOMove(new Vector2(-_pos[1].position.x, _pos[1].position.y), duration).
            SetEase(Ease.InOutSine).
            OnStart(() =>
                {
                    main.duration = 0.75f;
                    ps.Play();
                    _bossCube.transform.DORotate(new Vector3(0, 1080, 0), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutSine);
                }
            ).
            OnComplete(() =>
                {
                    emission.enabled = false;
                    _particleTr.DetachChildren();
                    //��ʊO�ɉ����o��
                    ps.transform.position += Vector3.up * 1000;
                    Destroy(ps.gameObject, duration);
                    _bossCube.transform.DOPlay();
                }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    private void AttackPatternTwo()
    {
        float duration = 4.5f;
        ParticleSystem ps = Instantiate(_particlesDict["BurstShot"], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = ps.emission;
        int emitCount = (int)emission.GetBurst(0).count.constant;
        emission.enabled = false;
        float moveToPosTime = 0.5f;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(720, 0, 0), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => this.transform.DOMove(_pos[2].position, moveToPosTime))
            );
        duration -= moveToPosTime;
        for (int i = (int)duration; i > 0; i--)
        {
            if (i % 2 == 0)
            {
                _seq.Append(transform.DOMoveX(-_pos[2].position.x, 1).OnStart(() =>
                {
                    ps.Emit(emitCount);
                    _bossCube.transform.DORotate(new Vector3(0, 720, 720), 1, RotateMode.FastBeyond360).
                    SetEase(Ease.OutQuad);
                }
                ));
            }
            else
            {
                _seq.Append(transform.DOMoveX(_pos[2].position.x, 1).OnStart(() =>
                {
                    ps.Emit(emitCount);
                    _bossCube.transform.DORotate(new Vector3(0, -720, -720), 1, RotateMode.FastBeyond360).
                    SetEase(Ease.OutQuad); 
                }
                ));
            }
        }
        _seq.Append(this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                _particleTr.DetachChildren();
                //��ʊO�ɉ����o��
                ps.transform.position += Vector3.up * 1000;
                Destroy(ps.gameObject, duration);
                _bossCube.transform.DOPlay();
            }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    public void OnDeath()
    {
        float duration = 4f;
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        FindObjectOfType<GameManager>().IsTimeStop = true;
        _seAus.PlayOneShot(_deathChargeSE);
        _seq?.Kill();
        transform.DOKill();
        _bossCube.transform.DOKill();
        //_bossCube.transform.rotation = Quaternion.Euler(Vector3.zero);
        this.transform.position = new Vector2(0, _startPos.y);
        Destroy(_particleTr.gameObject);
        StartCoroutine(Flash());
        _bossCube.transform.DORotate(360 * Random.Range(4.6f, 5.1f) * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => StartCoroutine(Explode())
            );
        StartCoroutine(FindObjectOfType<LightRay>().EmitLightRay(duration, 10));
    }
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Flash());
        _bossCube.SetActive(false);
        FindObjectOfType<LightRay>().gameObject.SetActive(false);
        _seAus.PlayOneShot(_deathExplodeSE);
        Instantiate(_explodePrefab, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("StageClear");
    }

    private IEnumerator Flash()
    {
        var flash = GameObject.Find("FlashPanel").GetComponent<Image>();
        flash.enabled = true;
        yield return new WaitForSeconds(0.1f);
        flash.enabled = false;
    }

    private void OnDisable()
    {
        _seq?.Kill();
        transform.DOKill();
        _bossCube.transform.DOKill();
    }
}
