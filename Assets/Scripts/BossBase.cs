using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// �{�X�̊��N���X
/// </summary>
public abstract class BossBase : MonoBehaviour
{
    [SerializeField, Header("�e���p�^�[��")]
    protected private GameObject[] _particles;

    [SerializeField, Header("���S���̉��i�`���[�W���j")]
    protected private AudioClip _deathChargeSE;

    [SerializeField, Header("���S���̉��i�����j")]
    protected private AudioClip _deathExplodeSE;

    [SerializeField, Header("���S���̃G�t�F�N�g�i�����j")]
    protected private GameObject _explodePrefab;

    [SerializeField, Header("���[�U�[��Prefab")]
    protected private GameObject _laser;

    [SerializeField, Header("���`�ԓ˓���ɑS�̂̐F��ς���ׂ̃p�l��")]
    protected private Image _effectImage;

    [SerializeField, Header("�{�X�̏��")]
    protected private BossState _state = BossState.Normal;

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
    /// <summary>��ʑS�̂����点��X�N���v�g</summary>
    protected private FlashEffect _flashEffector;
    /// <summary>Tween��ۑ����Ă��̃X�N���v�g���j�󂳂ꂽ�Ƃ���Tween���~�߂�p</summary>
    protected List<Tween> _tweens = new();
    /// <summary>��Փx</summary>
    protected private string _difficulty;

    /// <summary>
    /// �U���p�^�[���̒��I
    /// </summary>
    public void Attack()
    {
        if (_state == BossState.DebugAttack1)
        {
            AttackPatternOne();
        }
        else if (_state == BossState.DebugAttack2)
        {
            AttackPatternTwo();
        }
        else if (_state == BossState.DebugAttack3)
        {
            AttackPatternThree();
        }
        else
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
    }
    /// <summary>
    /// �U���p�^�[������1
    /// </summary>
    public abstract void AttackPatternOne();
    /// <summary>
    /// �U���p�^�[������2
    /// </summary>
    public abstract void AttackPatternTwo();
    /// <summary>
    /// �U���p�^�[������3
    /// </summary>
    public abstract void AttackPatternThree();
    /// <summary>
    /// �̗͔������̍U���p�^�[��
    /// </summary>
    public abstract void PhaseSecondStart();
    /// <summary>
    /// ���S���̉��o
    /// </summary>
    public abstract void Death();
    public enum BossState
    {
        Normal,
        DebugAttack1,
        DebugAttack2,
        DebugAttack3,
    }
    public void OnDisable()
    {
        _seq?.Kill();
        foreach (var t in _tweens)
        {
            t.Kill();
        }
    }
}
