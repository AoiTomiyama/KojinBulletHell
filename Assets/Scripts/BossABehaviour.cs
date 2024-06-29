using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossABehaviour : MonoBehaviour
{
    [Header("ƒ‰ƒCƒg")]
    [SerializeField]
    Light _light;
    private void Start()
    {
        transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear);
        transform.DOMove(new Vector3( - transform.position.x, transform.position.y), 3).
            SetLoops(-1, LoopType.Yoyo).
            SetEase(Ease.InOutQuad);
    }
}
