using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossABehaviour : MonoBehaviour
{
    /// <summary>TweenäÆóπëOÇ…ÉVÅ[Éìà⁄ìÆÇµÇΩç€Ç…KillÇ≈Ç´ÇÈÇÊÇ§Ç…ï€ë∂</summary>
    List<Tween> _tweens = new();
    private void Start()
    {
        _tweens.Add(
            transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear)
            );
        _tweens.Add(
            transform.DOMove(new Vector3( - transform.position.x, transform.position.y), 3).
            SetLoops(-1, LoopType.Yoyo).
            SetEase(Ease.InOutQuad)
            );
    }

    private void OnDisable()
    {
        foreach (var tw in _tweens)
        {
            tw.Kill();
        }
    }
}
