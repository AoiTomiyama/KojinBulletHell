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
        mousePosition.z = 0;    // Z 座標がカメラと同じになっているので、リセットする
        if (mousePosition.magnitude > 2)
        {
            this.transform.position = mousePosition;
        }
    }
}
