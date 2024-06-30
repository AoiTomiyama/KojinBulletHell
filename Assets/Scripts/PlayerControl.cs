using UnityEngine;

/// <summary>
/// プレイヤーの動きを制御するスクリプト。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    [Header("プレイヤーの移動速度")]
    [SerializeField]
    private float _moveSpeed = 1f;

    [Header("プレイヤーのジャンプ力")]
    [SerializeField]
    private float _jumpPower = 1f;

    [Header("何回ジャンプ可能か(-1でジャンプ回数∞)")]
    [SerializeField]
    private int _jumpCount = 2;

    [Header("二回目以降のジャンプ力")]
    [SerializeField]
    private float _jumpPowerAfterOneJump = 1f;

    [Header("発射時のSE")]
    [SerializeField]
    AudioClip _bulletShotSE;

    [Header("一段ジャンプのSE")]
    [SerializeField]
    AudioClip _oneJumpSE;

    [Header("二段ジャンプのSE")]
    [SerializeField]
    AudioClip _twoJumpSE;
    /// <summary> 残りあと何回ジャンプできるか </summary>
    private int _remainingJumpCount;
    /// <summary> 左右入力を取得 </summary>
    private float _h;
    /// <summary> ジャンプボタンが何秒押されたか </summary>
    private float _pressedJumpButtonTime;
    /// <summary> コンポーネントを取得 </summary>
    private Rigidbody2D _rb;
    /// <summary> ジャンプボタンが押されたかどうか </summary>
    private bool _isJumpPresed;
    /// <summary> 弾の発射口 </summary>
    private ParticleSystem _ps;
    /// <summary> 画面上の弾数</summary>
    private float _bulletCount;
    /// <summary> AudioSourceコンポーネントを取得</summary>
    private AudioSource _aus;
    /// <summary> ジャンプしたときにEmitさせるParticleSystem</summary>
    private ParticleSystem _jumpEffect;
    /// <summary> 敵体力を管理しているコンポーネントを取得 </summary>
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
