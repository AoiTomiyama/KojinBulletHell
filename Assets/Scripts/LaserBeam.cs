using DG.Tweening;
using UnityEngine;
/// <summary>
/// レーザービームを撃つスクリプト
/// </summary>

public class LaserBeam : MonoBehaviour, IPausable
{
    [SerializeField, Header("レーザーの幅")]
    private float _laserWidth = 30f;

    [SerializeField, Header("レーザーの展開秒数")]
    private float _startLaser = 0.5f;

    [SerializeField, Header("レーザーの停止秒数")]
    private float _endLaser = 0.5f;

    [SerializeField, Header("レーザーの継続秒数")]
    private float _laserDuration = 0.1f;

    [SerializeField, Header("予測線の継続秒数")]
    private float _prewarnDuration = 0.5f;

    [SerializeField, Header("レーザーの火力")]
    private int _damage = 3;

    [SerializeField, Header("プレイヤーを狙うか")]
    private bool _isTargetAtPlayer = false;

    [SerializeField, Header("予測線のSE")]
    private AudioClip _warnSE;

    [SerializeField, Header("発射時のSE")]
    private AudioClip _beamSE;

    /// <summary> 予測線のLineRendererを取得 </summary>
    private LineRenderer _prewarnLr;
    /// <summary> メインレーザーのLineRendererを取得 </summary>
    private LineRenderer _laserLr;
    /// <summary> レーザーが狙う敵のTransformを取得 </summary>
    private Transform _targetPos;
    /// <summary> レーザーの終点となる座標 </summary>
    private readonly Vector2 _endPos = Vector3.up * 100;
    /// <summary> 当たり判定</summary>
    private BoxCollider2D _hitBox;
    /// <summary> 体力を管理しているHealthControllerを取得 </summary>
    private HealthController _healthController;
    /// <summary> Tweenを保存してこのスクリプトが破壊されたときにTweenを止める用 </summary>
    private Tween _tweener;
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

        var player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            _targetPos = player.transform;
        }
        PrewarnLaser();
    }
    private void PrewarnLaser()
    {
        if (_isTargetAtPlayer && _targetPos != null)
        {
            transform.up = _targetPos.position - this.transform.position;
        }
        _seAus.PlayOneShot(_warnSE);
        _prewarnLr.SetPosition(0, Vector2.zero);
        _prewarnLr.SetPosition(1, _endPos);
        ShootLaser();
    }
    private void ShootLaser()
    {
        _tweener = DOTween.To(() => _laserLr.widthMultiplier, x => _laserLr.widthMultiplier = x, _laserWidth, _startLaser)
            .SetDelay(_prewarnDuration)
            .OnStart(() =>
            {
                FindObjectOfType<CameraShaker>().Shake(_damage / 3f, _startLaser, _laserDuration, _endLaser);

                if (_seAus.clip != _beamSE)
                {
                    _seAus.clip = _beamSE;
                }
                _seAus.Play();

                _laserLr.SetPosition(0, Vector2.zero);
                _laserLr.SetPosition(1, _endPos);
                _hitBox.enabled = _laserLr.enabled = true;
                _prewarnLr.SetPosition(1, Vector2.zero);
                GetComponent<ParticleSystem>().Emit(1);
            })
            .OnUpdate(SetCollider)
            .OnComplete(() =>
            {
                _tweener = DOTween.To(() => _laserLr.widthMultiplier, x => _laserLr.widthMultiplier = x, 0, _endLaser)
                .OnUpdate(SetCollider)
                .SetDelay(_laserDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => Destroy(this.gameObject));
            });
    }

    private void SetCollider()
    {
        const float lineWidthToColliderMultiplier = 0.15f;

        Vector2 midPos = _endPos / 2;
        float length = Vector2.Distance(Vector2.zero, _endPos);
        Vector2 direction = transform.up.normalized;
        _hitBox.size = new Vector2(_laserLr.widthMultiplier * lineWidthToColliderMultiplier, length);
        _hitBox.transform.localPosition = midPos;
        _hitBox.transform.up = direction;
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

    public void Pause()
    {
        _tweener?.Pause();
    }

    public void Resume()
    {
        _tweener?.Play();
    }
}
