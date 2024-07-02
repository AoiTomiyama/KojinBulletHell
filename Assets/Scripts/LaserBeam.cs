using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("レーザーの幅")]
    [SerializeField]
    float _laserWidth = 30f;
    [Header("レーザーの展開秒数")]
    [SerializeField]
    float _startLaser = 0.5f;
    [Header("レーザーの停止秒数")]
    [SerializeField]
    float _endLaser = 0.5f;
    [Header("レーザーの継続秒数")]
    [SerializeField]
    float _laserDuration = 0.1f;
    LineRenderer _lr;
    Transform _startPos;
    Transform _targetPos;
    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _startPos = GameObject.Find("BossACore").transform;
        _targetPos = GameObject.Find("Player").transform;
        ShootLaser();
    }
    private void ShootLaser()
    {
        _lr.SetPosition(0, Vector2.zero);
        Vector2 distance = _targetPos.position - _startPos.position;
        _lr.SetPosition(1, distance * 10);
        DOVirtual.Float(0, _laserWidth, _startLaser, (value) => _lr.widthMultiplier = value).
            OnComplete(
            () =>
            {
                DOVirtual.Float(_laserWidth, 0, _endLaser, (value) => _lr.widthMultiplier = value).
                SetEase(Ease.InQuad).
                SetDelay(_laserDuration).
                OnComplete(() => ShootLaser());
            }
            );
    }
}
