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
    /// <summary>行動パターンの保管</summary>
    Action[] _actions;
    ParticleSystem _ps;
    private void Start()
    {
        _pos = GameObject.Find("Positions").transform.GetComponentsInChildren<Transform>();
        _actions = new Action[1];
        _actions[0] = AttackPatternOne;
        _tween = transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear).OnStart(() =>
            {
                transform.DOMove(new Vector3(-transform.position.x, transform.position.y), 3).
                SetLoops(2, LoopType.Yoyo).
                SetEase(Ease.InOutQuad).
                OnComplete(() =>
                {
                    transform.DOPause();
                    _actions[Random.Range(0, _actions.Length)].Invoke();
                });
            }
            );
    }
    private void AttackPatternOne()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0, 720, 0), 0.5f, RotateMode.FastBeyond360).
            OnStart(() => transform.DOMove(_pos[1].position, 0.5f))
            );
        seq.Append(transform.DOMoveX(-transform.position.x, 5).OnStart(() =>
                    {
                        _ps = Instantiate(_particles[0], transform.position, Quaternion.identity, this.transform).GetComponent<ParticleSystem>();
                        transform.DORotate(new Vector3(0, 0, 720), 5, RotateMode.FastBeyond360);
                    }
                ).OnComplete(() =>
                {
                    var emission = _ps.emission;
                    emission.enabled = false;
                    Destroy(_ps.gameObject, 5);
                    transform.DOPlay();
                    transform.DOMove(new Vector3(-transform.position.x, transform.position.y), 3).
                        SetLoops(2, LoopType.Yoyo).
                        SetEase(Ease.InOutQuad).
                        OnComplete(() =>
                        {
                            transform.DOPause();
                            _actions[Random.Range(0, _actions.Length)].Invoke();
                        });
                })
            );
    }

    private void OnDisable()
    {
        _tween.Kill();
    }
}
