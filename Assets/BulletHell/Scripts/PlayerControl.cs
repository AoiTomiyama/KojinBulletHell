using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("�v���C���[�̈ړ����x")]
    [SerializeField]
    private float _moveSpeed = 1f;
    private float _v;
    private float _h;
    

    // Update is called once per frame
    void Update()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (_v != 0 || _h != 0)
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
            //���[�����͂��������̎��A�㉺���͂𖳌����B
            if (transform.position.y < screen_LeftBottom.y && _v <= -1)
            {
                _v = 0;
                Debug.Log("���[�ɓ��B");
            }
            //��[�����͂�������̎��A�㉺���͂𖳌����B
            if (transform.position.y > screen_RightTop.y && _v >= 1)
            {
                _v = 0;
                Debug.Log("��[�ɓ��B");
            }
            transform.position += new Vector3(_h, _v) * _moveSpeed;
        }
    }
}
