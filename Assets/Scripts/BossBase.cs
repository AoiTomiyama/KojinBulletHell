using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// �{�X�̊��N���X
/// </summary>
public abstract class BossBase : MonoBehaviour
{
    [Header("�e���p�^�[��")]
    [SerializeField]
    protected private GameObject[] _particles;
    [Header("���S���̉��i�`���[�W���j")]
    [SerializeField]
    protected private AudioClip _deathChargeSE;
    [Header("���S���̉��i�����j")]
    [SerializeField]
    protected private AudioClip _deathExplodeSE;
    [Header("���S���̃G�t�F�N�g�i�����j")]
    [SerializeField]
    protected private GameObject _explodePrefab;
    [Header("���[�U�[��Prefab")]
    [SerializeField]
    protected private GameObject _laser;

    /// <summary>�ړ���̏ꏊ</summary>
    protected private Transform[] _pos;
    /// <summary>�U�����̓���������B�����O�ɃV�[���ړ������ۂ�Kill�ł���悤�ɕۑ�</summary>
    protected private Sequence _seq;
    /// <summary>�{�X�̌����ڕ����B</summary>
    protected private GameObject _bossCube;
    /// <summary>�J�n���̈ʒu</summary>
    protected private Vector2 _startPos;
    /// <summary>�e���𔭐�������ʒu</summary>
    protected private Transform _particleTr;
    /// <summary>SE��炷���߂�AudioSource���擾</summary>
    protected private AudioSource _seAus;
    /// <summary>�e���p�^�[��</summary>
    protected private ParticleSystem _particlePattern;
    /// <summary>�V�[���h��GameObject</summary>
    protected private GameObject _shield;
    /// <summary>Tween��ۑ����Ă��̃X�N���v�g���j�󂳂ꂽ�Ƃ���Tween���~�߂�p</summary>
    protected List<Tween> _tweens = new();
    /// <summary>��Փx</summary>
    protected private string _difficulty;
    public void Attack()
    {
        int index = Random.Range(0, 3);
        if (index == 0)
        {
            AttackPatternOne();
        }
        else if (index == 1)
        {
            AttackPatternTwo();
        }
        else
        {
            AttackPatternThree();
        }
    }
    public abstract void AttackPatternOne();
    public abstract void AttackPatternTwo();
    public abstract void AttackPatternThree();
    public abstract void PhaseSecondStart();
    public abstract void Death();
    public void OnDisable()
    {
        _seq?.Kill();
        foreach (var t in _tweens)
        {
            t.Kill();
        }
    }
}