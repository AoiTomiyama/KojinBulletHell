using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾幕のヒット時に体力を減らしたり、生成時に音を鳴らしたりするスクリプト
/// ParticleManagerに統合予定。
/// </summary>
public class ParticleHit : MonoBehaviour
{
    [Header("パーティクルのダメージ量")]
    [SerializeField]
    int _particleDamage = 1;

    [Header("パーティクル発射時の音")]
    [SerializeField]
    AudioClip _particleShootSE;

    /// <summary> 体力を管理しているHealthControllerを取得 </summary>
    HealthController _healthController;

    /// <summary> 音源となるAudioSourceを取得 </summary>
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
