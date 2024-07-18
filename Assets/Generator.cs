using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    GameObject[] _gos;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Instantiate(_gos[0], transform.position, Quaternion.identity, transform);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Instantiate(_gos[1], transform.position, Quaternion.identity, transform);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(_gos[2], transform.position, Quaternion.identity, transform);
        }
    }
}
