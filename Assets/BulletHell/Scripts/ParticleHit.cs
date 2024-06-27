using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e���̃q�b�g���ɑ̗͂����炵����A�������ɉ���炵���肷��X�N���v�g
/// ParticleManager�ɓ����\��B
/// </summary>
public class ParticleHit : MonoBehaviour
{
    [Header("�p�[�e�B�N���̃_���[�W��")]
    [SerializeField]
    int _particleDamage = 1;

    [Header("�p�[�e�B�N�����ˎ��̉�")]
    [SerializeField]
    AudioClip _particleShootSE;

    /// <summary> �̗͂��Ǘ����Ă���HealthController���擾 </summary>
    HealthController _healthController;

    /// <summary> �����ƂȂ�AudioSource���擾 </summary>
    AudioSource _aus;
    private void Start()
    {
        _healthController = FindObjectOfType<HealthController>();
        _aus = GameObject.Find("SE").GetComponent<AudioSource>();
    }

    private void OnParticleTrigger()
    {
        _aus.PlayOneShot(_particleShootSE);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Player")
        {
            _healthController.RemoveHealth(_particleDamage);
        }
    }
}
