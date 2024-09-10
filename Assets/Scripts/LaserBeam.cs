using DG.Tweening;
using UnityEngine;
/// <summary>
/// ���[�U�[�r�[�������X�N���v�g
/// </summary>

public class LaserBeam : MonoBehaviour, IPausable
{
    [SerializeField, Header("���[�U�[�̕�")]
    private float _laserWidth = 30f;

    [SerializeField, Header("���[�U�[�̓W�J�b��")]
    private float _startLaser = 0.5f;

    [SerializeField, Header("���[�U�[�̒�~�b��")]
    private float _endLaser = 0.5f;

    [SerializeField, Header("���[�U�[�̌p���b��")]
    private float _laserDuration = 0.1f;

    [SerializeField, Header("�\�����̌p���b��")]
    private float _prewarnDuration = 0.5f;

    [SerializeField, Header("���[�U�[�̉Η�")]
    private int _damage = 3;

    [SerializeField, Header("�v���C���[��_����")]
    private bool _isTargetAtPlayer = false;

    [SerializeField, Header("�\������SE")]
    private AudioClip _warnSE;

    [SerializeField, Header("���ˎ���SE")]
    private AudioClip _beamSE;

    /// <summary> �\������LineRenderer���擾 </summary>
    private LineRenderer _prewarnLr;
    /// <summary> ���C�����[�U�[��LineRenderer���擾 </summary>
    private LineRenderer _laserLr;
    /// <summary> ���[�U�[���_���G��Transform���擾 </summary>
    private Transform _targetPos;
    /// <summary> ���[�U�[�̏I�_�ƂȂ���W </summary>
    private readonly Vector2 _endPos = Vector3.up * 100;
    /// <summary> �����蔻��</summary>
    private BoxCollider2D _hitBox;
    /// <summary> �̗͂��Ǘ����Ă���HealthController���擾 </summary>
    private HealthController _healthController;
    /// <summary> Tween��ۑ����Ă��̃X�N���v�g���j�󂳂ꂽ�Ƃ���Tween���~�߂�p </summary>
    private Tween _tweener;
    /// <summary>SE��Audiosource���擾</summary>
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
