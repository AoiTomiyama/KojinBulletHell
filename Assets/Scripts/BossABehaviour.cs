using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    Dictionary<string, GameObject> _particlesDict;
    Transform[] _pos;
    /// <summary>�U���p�^�[��������B�����O�ɃV�[���ړ������ۂ�Kill�ł���悤�ɕۑ�</summary>
    Sequence _seq;
    /// <summary>�s���p�^�[���̕ۊ�</summary>
    Action[] _actions;
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
        _actions = new Action[1];
        _actions[0] = AttackPatternOne;
        _bossCube.transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear).OnStart(() =>
            {
                this.transform.DOMove(new Vector2(-_startPos.x, _startPos.y), 3).
                SetLoops(2, LoopType.Yoyo).
                SetEase(Ease.InOutQuad).
                OnComplete(() =>
                {
                    _bossCube.transform.DOPause();
                    _actions[Random.Range(0, _actions.Length)].Invoke();
                });
            }
            );
    }
    private void AttackPatternOne()
    {
        float duration = 4.5f;
        ParticleSystem ps = default;
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
                    ps = Instantiate(_particlesDict["ShotgunShot"], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
                    _bossCube.transform.DORotate(new Vector3(0, 1080, 0), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutSine);
                }
            ).
            OnComplete(() =>
                {
                    var emission = ps.emission;
                    emission.enabled = false;
                    _particleTr.DetachChildren();
                    Destroy(ps.gameObject, duration);
                    _bossCube.transform.DOPlay();
                }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f));
        _seq.Append(
            this.transform.DOMove(new Vector2(-_startPos.x, _startPos.y), 3).
            SetLoops(2, LoopType.Yoyo).
            SetEase(Ease.InOutQuad).
            OnComplete(() =>
                {
                    _bossCube.transform.DOPause();
                    _actions[Random.Range(0, _actions.Length)].Invoke();
                }
            ));
    }

    private void AttackPatternTwo()
    {

    }

    public void OnDeath()
    {
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        _seAus.PlayOneShot(_deathChargeSE);
        if (_seq != null)
        {
            _seq.Kill();
        }
        transform.DOKill();
        _bossCube.transform.DOKill();
        _bossCube.transform.rotation = Quaternion.Euler(Vector3.zero);
        this.transform.position = new Vector2(0, _startPos.y);
        Destroy(_particleTr.gameObject);
        StartCoroutine(Flash());
        _bossCube.transform.DORotate(Vector3.one * 360 * 4.8f, 4, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => this.transform.DOLocalMoveX(0, 0.2f).OnComplete(() => _seAus.PlayOneShot(_deathExplodeSE))
            );
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
        if (_seq != null)
        {
            _seq.Kill();
        }
        transform.DOKill();
        _bossCube.transform.DOKill();
    }
}
