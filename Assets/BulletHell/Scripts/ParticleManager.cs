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
    int _particleDamage = 1;

    [Header("弾幕の挙動")]
    [SerializeField]
    ParticleBehaviour _particleBehaviour = ParticleBehaviour.None;

    [Header("方向を変える周期（RandomDirection時のみ有効）")]
    [SerializeField] 
    float _waitTime;

    [Header("方向の最大値（RandomDirection時のみ有効）")]
    [SerializeField] 
    float _maxAngular = 360;

    [Header("方向の最小値（RandomDirection時のみ有効）")]
    [SerializeField]
    float _minAngular = 0;

    [Header("発射時の効果音")]
    [SerializeField]
    AudioClip _shootSE;

    /// <summary> 体力を管理しているHealthControllerを取得 </summary>
    HealthController _healthController;
    /// <summary> 音源となるAudioSourceを取得 </summary>
    AudioSource _aus;
    /// <summary> プレイヤーのGameObjectを取得 </summary>
    GameObject _player;

    private void Start()
    {
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
        _aus.PlayOneShot(_shootSE);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Player")
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
