using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの方向にむかせるスクリプト
/// ParticleManagerに統合予定。
/// </summary>
public class FollowCursor : MonoBehaviour
{
    /// <summary> プレイヤーのGameObjectを取得 </summary>
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
