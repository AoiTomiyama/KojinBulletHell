using Cinemachine;
using DG.Tweening;
using UnityEngine;
/// <summary>
/// レーザービームを撃つスクリプト
/// </summary>

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
    [Header("プレイヤーを狙うか")]
    [SerializeField]
    private bool _isTargetAtPlayer = false;
    [Header("予測線のSE")]
    [SerializeField]
    AudioClip _warnSE;
    [Header("発射時のSE")]
    [SerializeField]
    AudioClip _beamSE;

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
    /// <summary> Tweenerを保存してこのスクリプトが破壊されたときにTweenerを止める用 </summary>
    private Tweener _tweener;
    /// <summary>SEのAudiosourceを取得</summary>
    private AudioSource _seAus;
    private void Start()
    {
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _laserLr = GetComponent<LineRenderer>();
        _laserLr.widthMultiplier = 0;
        _hitBox = transform.Find("Hitbox").GetComponent<BoxCollider2D>();
        _prewarnLr = transform.Find("Prewarn").GetComponent<LineRenderer>();

        _healthController = FindObjectOfType<HealthController>();
        _targetPos = GameObject.Find("Player").transform;
        PrewarnLaser();
    }
    private void PrewarnLaser()
    {
        if (_isTargetAtPlayer)
        {
            transform.up = _targetPos.position - this.transform.position;
        }
        _seAus.PlayOneShot(_warnSE);
        _endPos = Vector3.up * 100;
        _prewarnLr.SetPosition(0, Vector2.zero);
        _prewarnLr.SetPosition(1, _endPos);
        Invoke(nameof(ShootLaser), _prewarnDuration);
    }
    private void ShootLaser()
    {
        CameraShaker.Instance.Shake(_damage / 2f, _startLaser, _laserDuration, _endLaser);
        _seAus.PlayOneShot(_beamSE);
          _laserLr.SetPosition(0, Vector2.zero);
        _laserLr.SetPosition(1, _endPos);
        _hitBox.enabled = _laserLr.enabled = true;
        _prewarnLr.SetPosition(1, Vector2.zero);
        GetComponent<ParticleSystem>().Emit(1);
        _tweener = DOVirtual.Float(0, _laserWidth, _startLaser, (value) => _currentLaserWidth = value).
            OnComplete(
            () =>
            {
                _tweener = DOVirtual.Float(_laserWidth, 0, _endLaser, (value) => _currentLaserWidth = value).
                SetEase(Ease.InQuad).
                SetDelay(_laserDuration).
                OnComplete(() =>
                {
                    _hitBox.enabled = _laserLr.enabled = false;
                    if (_isLoop)
                    {
                        PrewarnLaser();
                    }
                    else
                    {
                        Destroy(this.gameObject);
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
        if (_hitBox.enabled && _currentLaserWidth > 0.01f)
        {
            _laserLr.widthMultiplier = _currentLaserWidth;
            Vector2 midPos = _endPos / 2;
            float length = Vector2.Distance(Vector2.zero, _endPos);
            Vector2 direction = transform.up.normalized;
            _hitBox.size = new Vector2(_currentLaserWidth * 0.15f, length);
            _hitBox.transform.localPosition = midPos;
            _hitBox.transform.up = direction;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _healthController.RemoveHealth(_damage);
        }
    }
    private void OnDisable()
    {
        _tweener?.Kill();
    }
}
