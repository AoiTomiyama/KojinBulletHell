using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("プレイヤーの移動速度")]
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
            //左端かつ入力が左方向の時、左右入力を無効化。
            if (transform.position.x < screen_LeftBottom.x && _h <= -1)
            {
                _h = 0;
                Debug.Log("左端に到達");
            }
            //右端かつ入力が右方向の時、左右入力を無効化。
            if (transform.position.x > screen_RightTop.x && _h >= 1)
            {
                _h = 0;
                Debug.Log("右端に到達");
            }
            //下端かつ入力が下方向の時、上下入力を無効化。
            if (transform.position.y < screen_LeftBottom.y && _v <= -1)
            {
                _v = 0;
                Debug.Log("下端に到達");
            }
            //上端かつ入力が上方向の時、上下入力を無効化。
            if (transform.position.y > screen_RightTop.y && _v >= 1)
            {
                _v = 0;
                Debug.Log("上端に到達");
            }
            transform.position += new Vector3(_h, _v) * _moveSpeed;
        }
    }
}
