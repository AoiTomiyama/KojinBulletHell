using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̕����ɂނ�����X�N���v�g
/// ParticleManager�ɓ����\��B
/// </summary>
public class FollowCursor : MonoBehaviour
{
    /// <summary> �v���C���[��GameObject���擾 </summary>
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
