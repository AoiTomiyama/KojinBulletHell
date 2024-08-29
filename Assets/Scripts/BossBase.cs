using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// ボスの基底クラス
/// </summary>
public abstract class BossBase : MonoBehaviour
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

    [SerializeField, Header("ボスの状態")]
    protected private BossState _state = BossState.Normal;

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
    protected private string _difficulty;

    /// <summary>
    /// 攻撃パターンの抽選
    /// </summary>
    public void Attack()
    {
        if (_state == BossState.DebugAttack1)
        {
            AttackPatternOne();
        }
        else if (_state == BossState.DebugAttack2)
        {
            AttackPatternTwo();
        }
        else if (_state == BossState.DebugAttack3)
        {
            AttackPatternThree();
        }
        else
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
            else
            {
                AttackPatternThree();
            }
        }
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
    public abstract void PhaseSecondStart();
    /// <summary>
    /// 死亡時の演出
    /// </summary>
    public abstract void Death();
    public enum BossState
    {
        Normal,
        DebugAttack1,
        DebugAttack2,
        DebugAttack3,
    }
    public void OnDisable()
    {
        _seq?.Kill();
        foreach (var t in _tweens)
        {
            t.Kill();
        }
    }
}
