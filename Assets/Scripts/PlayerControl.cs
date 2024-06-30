using UnityEngine;

/// <summary>
/// �v���C���[�̓����𐧌䂷��X�N���v�g�B
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    [Header("�v���C���[�̈ړ����x")]
    [SerializeField]
    private float _moveSpeed = 1f;

    [Header("�v���C���[�̃W�����v��")]
    [SerializeField]
    private float _jumpPower = 1f;

    [Header("����W�����v�\��(-1�ŃW�����v�񐔁�)")]
    [SerializeField]
    private int _jumpCount = 2;

    [Header("���ڈȍ~�̃W�����v��")]
    [SerializeField]
    private float _jumpPowerAfterOneJump = 1f;

    [Header("���ˎ���SE")]
    [SerializeField]
    AudioClip _bulletShotSE;

    [Header("��i�W�����v��SE")]
    [SerializeField]
    AudioClip _oneJumpSE;

    [Header("��i�W�����v��SE")]
    [SerializeField]
    AudioClip _twoJumpSE;
    /// <summary> �c�肠�Ɖ���W�����v�ł��邩 </summary>
    private int _remainingJumpCount;
    /// <summary> ���E���͂��擾 </summary>
    private float _h;
    /// <summary> �W�����v�{�^�������b�����ꂽ�� </summary>
    private float _pressedJumpButtonTime;
    /// <summary> �R���|�[�l���g���擾 </summary>
    private Rigidbody2D _rb;
    /// <summary> �W�����v�{�^���������ꂽ���ǂ��� </summary>
    private bool _isJumpPresed;
    /// <summary> �e�̔��ˌ� </summary>
    private ParticleSystem _ps;
    /// <summary> ��ʏ�̒e��</summary>
    private float _bulletCount;
    /// <summary> AudioSource�R���|�[�l���g���擾</summary>
    private AudioSource _aus;
    /// <summary> �W�����v�����Ƃ���Emit������ParticleSystem</summary>
    private ParticleSystem _jumpEffect;
    /// <summary> �G�̗͂��Ǘ����Ă���R���|�[�l���g���擾 </summary>
    private EnemyHealthController _enemyHealthController;


    private void Start()
    {
        _ps = transform.GetComponentInChildren<ParticleSystem>();
        _rb = GetComponent<Rigidbody2D>();
        _aus = GetComponent<AudioSource>();
        _jumpEffect = transform.Find("JumpEffect").GetComponent<ParticleSystem>();
        _enemyHealthController = FindObjectOfType<EnemyHealthController>();
        _aus.volume *= PlayerPrefs.GetFloat("SEVolume");
    }
    // Update is called once per frame
    void Update()
    {
        _h = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if (_remainingJumpCount > 0 || _jumpCount == -1)
            {
                _isJumpPresed = true;
                _remainingJumpCount--;
                _jumpEffect.Emit(1);
                if (_remainingJumpCount == _jumpCount - 1)
                {
                    _aus.PlayOneShot(_oneJumpSE);
                }
                else
                {
                    _aus.PlayOneShot(_twoJumpSE);
                }
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            _pressedJumpButtonTime = 0;
            _isJumpPresed = false;
        }
        if (_isJumpPresed)
        {
            _pressedJumpButtonTime += Time.deltaTime;
        }
        if (Input.GetButtonDown("Fire1") && _bulletCount < 3)
        {
            _ps.Emit(1);
            _aus.PlayOneShot(_bulletShotSE);
            _bulletCount++;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_h * _moveSpeed, _rb.velocity.y);
        if (_h != 0)
        {
            transform.localScale = new Vector3(_h, transform.localScale.y, transform.localScale.z);
        }

        if (_isJumpPresed && _pressedJumpButtonTime < 0.2f)
        {
            _rb.velocity = (_remainingJumpCount == _jumpCount - 1) ? new Vector2(_rb.velocity.x, _jumpPower) : new Vector2(_rb.velocity.x, _jumpPowerAfterOneJump);
        }
        else
        {
            _isJumpPresed = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Floor"))
        {
            _remainingJumpCount = _jumpCount;
            Debug.Log("On Floor");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Floor") && _remainingJumpCount == _jumpCount)
        {
            _remainingJumpCount--;
            Debug.Log("Leave Floor");
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.name.Contains("Boss"))
        {
            _enemyHealthController.EnemyDamage(1);
        }
        if (_bulletCount > 0)
        {
            _bulletCount--;
        }
    }
}
