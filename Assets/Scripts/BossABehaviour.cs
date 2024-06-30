using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossABehaviour : MonoBehaviour
{
    [Header("弾幕パターン")]
    [SerializeField]
    GameObject[] _particles;

    Transform[] _pos;
    /// <summary>Tween完了前にシーン移動した際にKillできるように保存</summary>
    Tween _tween;
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
    private void Start()
    {
        _startPos = this.transform.position;
        _particleTr = this.transform.Find("ParticlePosition").transform;
        _bossCube = this.transform.Find("BossCube").gameObject;
        _pos = GameObject.Find("Positions").transform.GetComponentsInChildren<Transform>();
        _actions = new Action[1];
        _actions[0] = AttackPatternOne;
        _tween = _bossCube.transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
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
        float duration = 5f;
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
                    ps = Instantiate(_particles[0], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
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

    private void OnDisable()
    {
        _tween.Kill();
        _seq.Kill();
    }
}
