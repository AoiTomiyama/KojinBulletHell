using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e���̃q�b�g���ɑ̗͂��������A�e���������Ɍ��ʉ���炷�X�N���v�g�B
/// FollowCursor�̃v���C���[�̕����������@�\�ƁARandomDirection�̃����_���ȕ����Ɍ����@�\�𓝍�����1�̃X�N���v�g�ɂ܂Ƃ߂��B�B
/// </summary>
public class ParticleManager : MonoBehaviour
{
    [Header("�p�[�e�B�N���̃_���[�W��")]
    [SerializeField]
    int _particleDamage = 1;

    [Header("�e���̋���")]
    [SerializeField]
    ParticleBehaviour _particleBehaviour = ParticleBehaviour.None;

    [Header("������ς�������iRandomDirection���̂ݗL���j")]
    [SerializeField]
    float _waitTime;

    [Header("�����̍ő�l�iRandomDirection���̂ݗL���j")]
    [SerializeField]
    float _maxAngular = 360;

    [Header("�����̍ŏ��l�iRandomDirection���̂ݗL���j")]
    [SerializeField]
    float _minAngular = 0;

    [Header("���ˎ��̌��ʉ�")]
    [SerializeField]
    AudioClip _shootSE;

    /// <summary> �̗͂��Ǘ����Ă���HealthController���擾 </summary>
    HealthController _healthController;
    /// <summary> �����ƂȂ�AudioSource���擾 </summary>
    AudioSource _aus;
    /// <summary> �v���C���[��GameObject���擾 </summary>
    GameObject _player;
    /// <summary> SE���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    float _seVolume;
    /// <summary> ParticleSystem���擾 </summary>
    ParticleSystem _ps;
    /// <summary> OnParticleTrigger�Ō��m����Particle��ۑ�����</summary>
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
