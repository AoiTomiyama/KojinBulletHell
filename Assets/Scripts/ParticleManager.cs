using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾幕のヒット時に体力を削ったり、弾幕生成時に効果音を鳴らすスクリプト。
/// FollowCursorのプレイヤーの方向を向く機能と、RandomDirectionのランダムな方向に向く機能を統合させ1つのスクリプトにまとめた。。
/// </summary>
public class ParticleManager : MonoBehaviour
{
    [Header("パーティクルのダメージ量")]
    [SerializeField]
    private int _particleDamage = 1;

    [Header("弾幕の挙動")]
    [SerializeField]
    private ParticleBehaviour _particleBehaviour = ParticleBehaviour.None;

    [Header("方向を変える周期（RandomDirection時のみ有効）")]
    [SerializeField]
    private float _waitTime;

    [Header("方向の最大値（RandomDirection時のみ有効）")]
    [SerializeField]
    private float _maxAngular = 360;

    [Header("方向の最小値（RandomDirection時のみ有効）")]
    [SerializeField]
    private float _minAngular = 0;

    [Header("発射時の効果音")]
    [SerializeField]
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
    private HashSet<int> triggeredParticles = new();

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _seVolume = PlayerPrefs.GetFloat("SEVolume");
        _healthController = FindObjectOfType<HealthController>();
        _aus = GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
        if (_particleBehaviour == ParticleBehaviour.RandomDirection)
        {
            StartCoroutine(RandomRotateRepeat());
        }
    }

    private void FixedUpdate()
    {
        if (_particleBehaviour == ParticleBehaviour.FollowPlayer)
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
    IEnumerator RandomRotateRepeat()
    {
        while (true)
        {
            float randomZRotation = Random.Range(_minAngular, _maxAngular);
            transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);
            yield return new WaitForSeconds(_waitTime);
        }
    }

    enum ParticleBehaviour
    {
        None,
        FollowPlayer,
        RandomDirection,
    }
}
