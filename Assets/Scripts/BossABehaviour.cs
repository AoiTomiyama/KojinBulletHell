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
    [Header("弾幕パターン")]
    [SerializeField]
    GameObject[] _particles;
    [Header("死亡時の音（チャージ音）")]
    [SerializeField]
    AudioClip _deathChargeSE;
    [Header("死亡時の音（爆発）")]
    [SerializeField]
    AudioClip _deathExplodeSE;
    [Header("死亡時のエフェクト（爆発）")]
    [SerializeField]
    GameObject _explodePrefab;

    Dictionary<string, GameObject> _particlesDict;
    Transform[] _pos;
    /// <summary>攻撃パターンを入れる。完了前にシーン移動した際にKillできるように保存</summary>
    Sequence _seq;
    /// <summary>行動パターンの保管</summary>
    Action[] _actions;
    /// <summary>ボスの見た目部分。</summary>
    GameObject _bossCube;
    /// <summary>開始時の位置</summary>
    Vector2 _startPos;
    /// <summary>弾幕を発生させる位置</summary>
    Transform _particleTr;
    /// <summary>SEを鳴らすためのAudioSourceを取得</summary>
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
                    //画面外に押し出す
                    ps.transform.position += Vector3.up * 1000;
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
