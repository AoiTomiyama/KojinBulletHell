using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// ボスAの動き
/// </summary>
public class BossABehaviour : MonoBehaviour
{
    [Header("弾幕パターン")]
    [SerializeField]
    private GameObject[] _particles;
    [Header("死亡時の音（チャージ音）")]
    [SerializeField]
    private AudioClip _deathChargeSE;
    [Header("死亡時の音（爆発）")]
    [SerializeField]
    private AudioClip _deathExplodeSE;
    [Header("死亡時のエフェクト（爆発）")]
    [SerializeField]
    private GameObject _explodePrefab;
    [Header("シールド展開時のSprite")]
    [SerializeField]
    private Sprite _shieldEnabled;

    /// <summary>移動先の場所</summary>
    private Transform[] _pos;
    /// <summary>攻撃時の動きを入れる。完了前にシーン移動した際にKillできるように保存</summary>
    private Sequence _seq;
    /// <summary>ボスの見た目部分。</summary>
    private GameObject _bossCube;
    /// <summary>開始時の位置</summary>
    private Vector2 _startPos;
    /// <summary>弾幕を発生させる位置</summary>
    private Transform _particleTr;
    /// <summary>SEを鳴らすためのAudioSourceを取得</summary>
    private AudioSource _seAus;
    /// <summary> 弾幕パターン</summary>
    private ParticleSystem _particlePattern;
    /// <summary> シールドのGameObject</summary>
    private GameObject _shield;
    private void Start()
    {
        _shield = transform.Find("Shield").gameObject;
        _shield.SetActive(false);
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
            }
            );
    }
    private void Attack()
    {
        int index = Random.Range(0, 3);
        if (index == 0)
        {
            AttackPatternOne();
        }
        else if (index == 1)
        {
            AttackPatternTwo();
        }
        else if (index == 2)
        {
            AttackPatternThree();
        }
        else
        {
            Debug.Log("Attack failed");
        }
    }
    private void AttackPatternOne()
    {
        float duration = 4.5f;
        _particlePattern = Instantiate(_particles[0], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var emission = _particlePattern.emission;
        var main = _particlePattern.main;
        main.duration = 0.75f;
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
                    _particlePattern.Play();
                    _bossCube.transform.DORotate(new Vector3(0, 360 * 5, 360), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutSine);
                }
            ).
            OnComplete(() =>
                {
                    emission.enabled = false;
                    _particleTr.DetachChildren();
                    Destroy(_particlePattern.gameObject, duration);
                    _bossCube.transform.DOPlay();
                }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    private void AttackPatternTwo()
    {
        float duration = 4f;
        float moveToPosTime = 0.5f;
        _particlePattern = Instantiate(_particles[1], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;
        int emitCount = (int)emission.GetBurst(0).count.constant;
        emission.enabled = false;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(720, 0, 0), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => this.transform.DOMove(_pos[2].position, moveToPosTime))
            );
        for (int i = (int)duration; i > 0; i--)
        {
            if (i % 2 == 0)
            {
                _seq.Append(transform.DOMoveX(-_pos[2].position.x, 1).OnStart(() =>
                {
                    _particlePattern.Emit(emitCount);
                    _bossCube.transform.DORotate(new Vector3(0, 720, 720), 1, RotateMode.FastBeyond360).
                    SetEase(Ease.OutQuad);
                }
                ));
            }
            else
            {
                _seq.Append(transform.DOMoveX(_pos[2].position.x, 1).OnStart(() =>
                {
                    _particlePattern.Emit(emitCount);
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
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }
    private void AttackPatternThree()
    {
        float duration = 5f;
        _particlePattern = Instantiate(_particles[2], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var emission = _particlePattern.emission;
        emission.rateOverTimeMultiplier *= 2f;
        float moveToPosTime = 0.5f;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 0, 720), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => this.transform.DOMove(_pos[3].position, moveToPosTime))
            );
        _seq.Append(
            _bossCube.transform.DORotate(18000 * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            OnStart(() => _particlePattern.Play())
            );
        _seq.Append(this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    public void PhaseSecondStart()
    {
        Debug.Log("Phase 2 Start");
        StartCoroutine(Flash());
        _seq?.Kill();
        _bossCube.transform.DOPause();
        this.transform.DOKill();
        if (_particlePattern != null)
        {
            Destroy(_particlePattern.gameObject);
        }
        _shield.SetActive(true);
        Tween tw = _shield.transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear);
        this.transform.position = _pos[3].position;
        _bossCube.transform.rotation = Quaternion.Euler(0, 0, 90);
        _bossCube.transform.DORotate(Vector3.one * 3600, 5, RotateMode.FastBeyond360).
            SetEase(Ease.Linear).
            OnComplete(() =>
            {
                StartCoroutine(Flash());
                tw.Kill();
                _shield.SetActive(false);
                _bossCube.transform.DOPlay();
                this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove());
            }
            );
    }
    public void Death()
    {
        float duration = 4f;
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        FindObjectOfType<GameManager>().IsTimeStop = true;
        _seAus.PlayOneShot(_deathChargeSE);
        _seq?.Kill();
        transform.DOKill();
        _bossCube.transform.DOKill();
        this.transform.position = new Vector2(0, _startPos.y);
        Destroy(_particleTr.gameObject);
        StartCoroutine(Flash());
        _bossCube.transform.DORotate(360 * Random.Range(4.6f, 5.1f) * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => StartCoroutine(Explode()));
        StartCoroutine(FindObjectOfType<LightRay>().EmitLightRay(duration, 10));
    }
    private IEnumerator Explode()
    {
        var shaker = FindObjectOfType<CinemachineImpulseSource>();
        shaker.GenerateImpulseWithForce(5);
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
