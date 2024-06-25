using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPlayer : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;    // Z ���W���J�����Ɠ����ɂȂ��Ă���̂ŁA���Z�b�g����
        if (mousePosition.magnitude > 2)
        {
            this.transform.position = mousePosition;
        }
    }
}
