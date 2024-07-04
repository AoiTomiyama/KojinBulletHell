using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LaserBeam : MonoBehaviour
{
    [Header("レーザーの幅")]
    [SerializeField]
    private float _laserWidth = 30f;
    [Header("レーザーの展開秒数")]
    [SerializeField]
    private float _startLaser = 0.5f;
    [Header("レーザーの停止秒数")]
    [SerializeField]
    private float _endLaser = 0.5f;
    [Header("レーザーの継続秒数")]
    [SerializeField]
    private float _laserDuration = 0.1f;
    [Header("予測線の継続秒数")]
    [SerializeField]
    private float _prewarnDuration = 0.5f;
    [Header("レーザーの火力")]
    [SerializeField]
    private int _damage = 3;
    [Header("ループさせるか")]
    [SerializeField]
    private bool _isLoop = false;

    /// <summary> 予測線のLineRendererを取得 </summary>
    private LineRenderer _prewarnLr;
    /// <summary> メインレーザーのLineRendererを取得 </summary>
    private LineRenderer _laserLr;
    /// <summary> レーザーが狙う敵のTransformを取得 </summary>
    private Transform _targetPos;
    /// <summary> 現在のレーザーの太さを取得 </summary>
    private float _currentLaserWidth;
    /// <summary> レーザーの終点となる座標 </summary>
    private Vector2 _endPos;
    /// <summary> 当たり判定</summary>
    private BoxCollider2D _hitBox;
    /// <summary> 体力を管理しているHealthControllerを取得 </summary>
    private HealthController _healthController;
    /// <summary> Tweenerを保存してこのスクリプトが破壊されたときにTweenを止める用 </summary>
    private Tweener _tweener;
    private void Start()
    {
        _laserLr = GetComponent<LineRenderer>();
        _hitBox = transform.Find("Hitbox").GetComponent<BoxCollider2D>();
        _prewarnLr = transform.Find("Prewarn").GetComponent<LineRenderer>();

        _healthController = FindObjectOfType<HealthController>();
        _targetPos = GameObject.Find("Player").transform;
        PrewarnLaser();
    }
    private void PrewarnLaser()
    {
        _endPos = (_targetPos.position - this.transform.position) * 5;
        _prewarnLr.SetPosition(0, Vector2.zero);
        _prewarnLr.SetPosition(1, _endPos);
        Invoke(nameof(ShootLaser), _prewarnDuration);
    }
    private void ShootLaser()
    {
        _laserLr.SetPosition(0, Vector2.zero);
        _laserLr.SetPosition(1, _endPos);
        _hitBox.enabled = _laserLr.enabled = true;
        _prewarnLr.SetPosition(1, Vector2.zero);
        _tweener = DOVirtual.Float(0, _laserWidth, _startLaser, (value) => _currentLaserWidth = value).
            OnComplete(
            () =>
            {
                DOVirtual.Float(_laserWidth, 0, _endLaser, (value) => _currentLaserWidth = value).
                SetEase(Ease.InQuad).
                SetDelay(_laserDuration).
                OnComplete(() =>
                {
                    _hitBox.enabled = _laserLr.enabled = false;
                    if (_isLoop)
                    {
                        PrewarnLaser();
                    }
                });
            }
            );
    }

    private void FixedUpdate()
    {
        SetCollider();
    }
    private void SetCollider()
    {
        if (_hitBox.enabled)
        {
            _laserLr.widthMultiplier = _currentLaserWidth;
            Vector2 midPos = _endPos / 2;
            float length = Vector2.Distance(Vector2.zero, _endPos);
            Vector2 direction = _endPos.normalized;
            _hitBox.size = new Vector2(_currentLaserWidth * 0.3f, length);
            _hitBox.transform.position = midPos + (Vector2)this.transform.position;
            _hitBox.transform.up = direction;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _healthController.RemoveHealth(_damage);
        }
    }
    private void OnDestroy()
    {
        _tweener?.Kill();
    }
}
