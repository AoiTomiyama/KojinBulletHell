using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField, Header("‰ñ“]‘¬“x")] private float _speed;
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * _speed);
    }
}
