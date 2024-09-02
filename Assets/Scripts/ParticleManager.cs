using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾幕のヒット時に体力を削ったり、弾幕生成時に効果音を鳴らすスクリプト。
/// 通常・自機狙いの二種類のタイプをもつ。
/// </summary>
public class ParticleManager : MonoBehaviour
{
    [SerializeField, Header("パーティクルのダメージ量")]
    private int _particleDamage = 1;

    [SerializeField, Header("弾幕の挙動")]
    private ParticleBehaviour _particleBehaviour = ParticleBehaviour.None;

    [SerializeField, Header("発射時の効果音")]
    private AudioClip _shootSE;

    /// <summary> 体力を管理しているHealthControllerを取得 </summary>
    private HealthController _healthController;
    /// <summary> 音源となるAudioSourceを取得 </summary>
    private AudioSource _aus;
    /// <summary> プレイヤーのGameObjectを取得 </summary>
    private GameObject _player;
    /// <summary> SE音量の値を一時的に入れる変数 </summary>
    private float _seVolume;
    /// <summary> ParticleSystemを取得 </summary>
    private ParticleSystem _ps;
    /// <summary> OnParticleTriggerで検知したParticleを保存する</summary>
    private readonly HashSet<int> triggeredParticles = new();

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _aus = GetComponent<AudioSource>();
        _healthController = FindObjectOfType<HealthController>();
        _player = FindObjectOfType<PlayerControl>().gameObject;
        _seVolume = PlayerPrefs.GetFloat("SEVolume");
    }

    private void FixedUpdate()
    {
        if (_particleBehaviour == ParticleBehaviour.FollowPlayer && _player != null)
        {
            transform.up = _player.transform.position - transform.position;
        }
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enterParticles = new();
        int enterCount = _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);
        bool hasTriggeredNewParticle = false;

        for (int i = 0; i < enterCount; i++)
        {
            int particleID = (int)enterParticles[i].randomSeed;

            if (!triggeredParticles.Contains(particleID))
            {
                triggeredParticles.Add(particleID);
                hasTriggeredNewParticle |= true;
            }
        }
        if (hasTriggeredNewParticle)
        {
            _aus.PlayOneShot(_shootSE, _aus.volume * _seVolume);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            _healthController.RemoveHealth(_particleDamage);
        }
    }

    enum ParticleBehaviour
    {
        None,
        FollowPlayer
    }
}
