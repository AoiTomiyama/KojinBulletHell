using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// ボスの基底クラス
/// </summary>
public abstract class BossBase : MonoBehaviour, IPausable
{
    [SerializeField, Header("弾幕パターン")]
    protected private GameObject[] _particles;

    [SerializeField, Header("死亡時の音（チャージ音）")]
    protected private AudioClip _deathChargeSE;

    [SerializeField, Header("死亡時の音（爆発）")]
    protected private AudioClip _deathExplodeSE;

    [SerializeField, Header("死亡時のエフェクト（爆発）")]
    protected private GameObject _explodePrefab;

    [SerializeField, Header("レーザーのPrefab")]
    protected private GameObject _laser;

    [SerializeField, Header("第二形態突入後に全体の色を変える為のパネル")]
    protected private Image _effectImage;

    [SerializeField, Header("最終攻撃")]
    protected private GameObject _finalAttack;

    [SerializeField, Header("最終攻撃時に展開する魔法陣")]
    protected private GameObject _magicEffector;

    [SerializeField, Header("ボスの状態")]
    protected private BossState _state = BossState.Normal;

    /// <summary>前回の攻撃パターンのインデックス</summary>
    protected private int _lastIndexOfAttack = -1;
    /// <summary>移動先の場所</summary>
    protected private Transform[] _pos;
    /// <summary>攻撃時の動きを入れる。完了前にシーン移動した際にKillできるように保存</summary>
    protected private Sequence _seq;
    /// <summary>ボスの見た目部分。</summary>
    protected private GameObject _bossCube;
    /// <summary>開始時の位置</summary>
    protected private Vector2 _startPos;
    /// <summary>弾幕を発生させる位置</summary>
    protected private Transform _particleTr;
    /// <summary>SEを鳴らすためのAudioSourceを取得</summary>
    protected private AudioSource _seAus;
    /// <summary>弾幕パターン</summary>
    protected private ParticleSystem _particlePattern;
    /// <summary>シールドのGameObject</summary>
    protected private GameObject _shield;
    /// <summary>画面全体を光らせるスクリプト</summary>
    protected private FlashEffect _flashEffector;
    /// <summary>Tweenを保存してこのスクリプトが破壊されたときにTweenを止める用</summary>
    protected List<Tween> _tweens = new();
    /// <summary>難易度</summary>
    protected private Enums.Difficulties _difficulty;
    
    /// <summary>
    /// Startが呼び出されたときに時の処理。
    /// </summary>
    public void OnStartSetUp()
    {
        _flashEffector = FindObjectOfType<FlashEffect>();
        _difficulty = (Enums.Difficulties)PlayerPrefs.GetInt("DIFF_INT");
        _shield = GameObject.FindWithTag("Shield");
        _shield.SetActive(false);
        _magicEffector.SetActive(false);
        _startPos = this.transform.position;
        _particleTr = this.transform.Find("ParticlePosition").transform;
        _bossCube = this.transform.Find("BossCube").gameObject;
        _pos = GameObject.Find("Positions").transform.GetComponentsInChildren<Transform>();
        _seAus = GetComponent<AudioSource>();
        _seAus.volume *= PlayerPrefs.GetFloat("SEVolume");
        _tweens.Add(
            _bossCube.transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear)
        );
        WanderingMove();
    }

    /// <summary>
    /// 攻撃パターンの抽選
    /// </summary>
    public void ChooseAttack()
    {
        if (_state == BossState.StartFinalAttackAtBeginning)
        {
            Debug.Log("<color=yellow>[Boss]</color> Final Attack Start");
            FinalAttack();
        }
        else if (_state == BossState.DebugAttack1)
        {
            Debug.Log("<color=yellow>[Boss]</color> Attack One Start");
            AttackPatternOne();
        }
        else if (_state == BossState.DebugAttack2)
        {
            Debug.Log("<color=yellow>[Boss]</color> Attack Two Start");
            AttackPatternTwo();
        }
        else if (_state == BossState.DebugAttack3)
        {
            Debug.Log("<color=yellow>[Boss]</color> Attack Three Start");
            AttackPatternThree();
        }
        else
        {
            int currentIndex = Random.Range(0, 3);

            //攻撃パターンが連続しないように再抽選を行う。
            while (currentIndex == _lastIndexOfAttack)
            {
                currentIndex = Random.Range(0, 3);
            }
            _lastIndexOfAttack = currentIndex;

            if (currentIndex == 0)
            {
                Debug.Log("<color=yellow>[Boss]</color> Attack One Start");
                AttackPatternOne();
            }
            else if (currentIndex == 1)
            {
                Debug.Log("<color=yellow>[Boss]</color> Attack Two Start");
                AttackPatternTwo();
            }
            else if (currentIndex == 2)
            {
                Debug.Log("<color=yellow>[Boss]</color> Attack Three Start");
                AttackPatternThree();
            }
            else
            {
                Debug.LogWarning("Attack pattern index was out of range!");
                ChooseAttack();
            }
        }
    }
    /// <summary>
    /// 攻撃後・攻撃前の徘徊移動
    /// </summary>
    public void WanderingMove()
    {
        _tweens.Add(
            this.transform.DOMove(new Vector2(-_startPos.x, _startPos.y), 3).
            SetLoops(2, LoopType.Yoyo).
            SetEase(Ease.InOutQuad).
            OnComplete(() =>
            {
                _bossCube.transform.DOPause();
                ChooseAttack();
            })
        );
    }
    /// <summary>
    /// 攻撃パターンその1
    /// </summary>
    public abstract void AttackPatternOne();
    /// <summary>
    /// 攻撃パターンその2
    /// </summary>
    public abstract void AttackPatternTwo();
    /// <summary>
    /// 攻撃パターンその3
    /// </summary>
    public abstract void AttackPatternThree();
    /// <summary>
    /// 体力半分時の攻撃パターン
    /// </summary>
    public virtual void FinalAttack()
    {
        Debug.LogError("<color=yellow>[BossBase]</color> Final attack isn't implemented!");
    }
    public abstract void PhaseSecondStart();
    /// <summary>
    /// 死亡時の演出
    /// </summary>
    public void Death()
    {
        const float duration = 4f;
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        FindObjectOfType<GameManager>().IsTimeStop = true;
        _seAus.PlayOneShot(_deathChargeSE);
        _seq?.Kill();
        transform.DOKill();
        _bossCube.transform.DOKill();
        this.transform.position = new Vector2(0, _startPos.y);
        Destroy(_particleTr.gameObject);
        _flashEffector.Flash();
        _tweens.Add(
            _bossCube.transform.DORotate(360 * Random.Range(4.6f, 5.1f) * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => StartCoroutine(Explode()))
        );
        StartCoroutine(FindObjectOfType<LightRay>().EmitLightRay(duration, 10));
    }
    /// <summary>
    /// 爆発エフェクト関連。
    /// </summary>
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<CameraShaker>().Shake(3, 0, 1, 0.4f);
        _flashEffector.Flash();
        _bossCube.SetActive(false);
        FindObjectOfType<LightRay>().gameObject.SetActive(false);
        _seAus.PlayOneShot(_deathExplodeSE);
        Instantiate(_explodePrefab, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.5f);
        FindObjectOfType<FadeInOut>().FadeInAndChangeScene("StageClear");
    }
    public enum BossState
    {
        Normal,
        DebugAttack1,
        DebugAttack2,
        DebugAttack3,
        StartFinalAttackAtBeginning,
    }
    public void OnDisable()
    {
        _seq?.Kill();
        foreach (var t in _tweens)
        {
            t.Kill();
        }
    }

    public abstract void Pause();
    public abstract void Resume();
}
