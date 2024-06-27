using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int _remainingJumpCount;
    private float _h;
    private float _pressedJumpButtonTime;
    private Rigidbody2D _rb;
    private bool _isJumpPresed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_h * _moveSpeed, _rb.velocity.y);

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
        if (collision.gameObject.name == "Floor")
        {
            _remainingJumpCount = _jumpCount;
        }
    }
}
