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
    [Header("弾幕パターン")]
    [SerializeField]
    protected private GameObject[] _particles;

    [Header("死亡時の音（チャージ音）")]
    [SerializeField]
    protected private AudioClip _deathChargeSE;

    [Header("死亡時の音（爆発）")]
    [SerializeField]
    protected private AudioClip _deathExplodeSE;

    [Header("死亡時のエフェクト（爆発）")]
    [SerializeField]
    protected private GameObject _explodePrefab;

    [Header("レーザーのPrefab")]
    [SerializeField]
    protected private GameObject _laser;

    [SerializeField, Header("第二形態突入後に全体の色を変える為のパネル")]
    protected private Image _effectImage;

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
    /// <summary>Tweenを保存してこのスクリプトが破壊されたときにTweenを止める用</summary>
    protected List<Tween> _tweens = new();
    /// <summary>難易度</summary>
    protected private string _difficulty;

    /// <summary>
    /// 攻撃パターンの抽選
    /// </summary>
    public void Attack()
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
    public void OnDisable()
    {
        _seq?.Kill();
        foreach (var t in _tweens)
        {
            t.Kill();
        }
    }
}
