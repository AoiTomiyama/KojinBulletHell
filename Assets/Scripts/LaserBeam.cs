using Cinemachine;
using DG.Tweening;
using UnityEngine;
/// <summary>
/// ���[�U�[�r�[�������X�N���v�g
/// </summary>

public class LaserBeam : MonoBehaviour
{
    [Header("���[�U�[�̕�")]
    [SerializeField]
    private float _laserWidth = 30f;
    [Header("���[�U�[�̓W�J�b��")]
    [SerializeField]
    private float _startLaser = 0.5f;
    [Header("���[�U�[�̒�~�b��")]
    [SerializeField]
    private float _endLaser = 0.5f;
    [Header("���[�U�[�̌p���b��")]
    [SerializeField]
    private float _laserDuration = 0.1f;
    [Header("�\�����̌p���b��")]
    [SerializeField]
    private float _prewarnDuration = 0.5f;
    [Header("���[�U�[�̉Η�")]
    [SerializeField]
    private int _damage = 3;
    [Header("���[�v�����邩")]
    [SerializeField]
    private bool _isLoop = false;
    [Header("�v���C���[��_����")]
    [SerializeField]
    private bool _isTargetAtPlayer = false;
    [Header("�\������SE")]
    [SerializeField]
    AudioClip _warnSE;
    [Header("���ˎ���SE")]
    [SerializeField]
    AudioClip _beamSE;

    /// <summary> �\������LineRenderer���擾 </summary>
    private LineRenderer _prewarnLr;
    /// <summary> ���C�����[�U�[��LineRenderer���擾 </summary>
    private LineRenderer _laserLr;
    /// <summary> ���[�U�[���_���G��Transform���擾 </summary>
    private Transform _targetPos;
    /// <summary> ���݂̃��[�U�[�̑������擾 </summary>
    private float _currentLaserWidth;
    /// <summary> ���[�U�[�̏I�_�ƂȂ���W </summary>
    private Vector2 _endPos;
    /// <summary> �����蔻��</summary>
    private BoxCollider2D _hitBox;
    /// <summary> �̗͂��Ǘ����Ă���HealthController���擾 </summary>
    private HealthController _healthController;
    /// <summary> Tweener��ۑ����Ă��̃X�N���v�g���j�󂳂ꂽ�Ƃ���Tweener���~�߂�p </summary>
    private Tweener _tweener;
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
