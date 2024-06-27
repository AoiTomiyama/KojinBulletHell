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
    private float _h;
    private Rigidbody2D _rb;
    private float _pressedJumpButtonTime;
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
            _isJumpPresed = true;
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
        Vector3 screen_LeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 screen_RightTop = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );
        //���[�����͂��������̎��A���E���͂𖳌����B
        if (transform.position.x < screen_LeftBottom.x && _h <= -1)
        {
            _h = 0;
            Debug.Log("���[�ɓ��B");
        }
        //�E�[�����͂��E�����̎��A���E���͂𖳌����B
        if (transform.position.x > screen_RightTop.x && _h >= 1)
        {
            _h = 0;
            Debug.Log("�E�[�ɓ��B");
        }
        _rb.velocity = new Vector2(_h * _moveSpeed, _rb.velocity.y);

        if (_isJumpPresed && _pressedJumpButtonTime < 0.2f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
        }
        else
        {
            _isJumpPresed = false;
        }
    }
}
