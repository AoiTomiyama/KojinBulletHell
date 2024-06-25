using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    GameObject _player;
    private void Start()
    {
        _player = GameObject.Find("Player");
    }
    void FixedUpdate()
    {
        transform.up = _player.transform.position - transform.position;
    }
}
