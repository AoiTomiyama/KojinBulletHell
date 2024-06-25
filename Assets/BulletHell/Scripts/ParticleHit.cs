using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    [Header("パーティクルのダメージ量")]
    [SerializeField]
    int _particleDamage = 1;
    HealthController _healthController;
    private void Start()
    {
        _healthController = FindObjectOfType<HealthController>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name == "Player")
        {
            _healthController.RemoveHealth(_particleDamage);
        }
    }
}
