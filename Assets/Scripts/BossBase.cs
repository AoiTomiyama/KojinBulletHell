using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// �{�X�̊��N���X
/// </summary>
public abstract class BossBase : MonoBehaviour, IPausable
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

    [SerializeField, Header("�ŏI�U��")]
    protected private GameObject _finalAttack;

    [SerializeField, Header("�ŏI�U�����ɓW�J���閂�@�w")]
    protected private GameObject _magicEffector;

    [SerializeField, Header("�{�X�̏��")]
    protected private BossState _state = BossState.Normal;

    /// <summary>�O��̍U���p�^�[���̃C���f�b�N�X</summary>
    protected private int _lastIndexOfAttack = -1;
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
    protected private Enums.Difficulties _difficulty;
    
    /// <summary>
    /// Start���Ăяo���ꂽ�Ƃ��Ɏ��̏����B
    /// </summary>
    public void OnStartSetUp()
    {
        _flashEffector = FindObjectOfType<FlashEffect>();
        _difficulty = (Enums.Difficulties)PlayerPrefs.GetInt("DIFF_INT");
        _shield = GameObject.FindWithTag("Shield");
        _shield.SetActive(false);
        _magicEffector.SetActive(false);
        _startPos = this.transform.position;
        _particleTr = this.transform.Find("ParticlePosition").transform;
        _bossCube = this.transform.Find("BossCube").gameObject;
        _pos = GameObject.Find("Positions").transform.GetComponentsInChildren<Transform>();
        _seAus = GetComponent<AudioSource>();
        _seAus.volume *= PlayerPrefs.GetFloat("SEVolume");
        _tweens.Add(
            _bossCube.transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear)
        );
        WanderingMove();
    }

    /// <summary>
    /// �U���p�^�[���̒��I
    /// </summary>
    public void ChooseAttack()
    {
        if (_state == BossState.StartFinalAttackAtBeginning)
        {
            Debug.Log("<color=yellow>[Boss]</color> Final Attack Start");
            FinalAttack();
        }
        else if (_state == BossState.DebugAttack1)
        {
            Debug.Log("<color=yellow>[Boss]</color> Attack One Start");
            AttackPatternOne();
        }
        else if (_state == BossState.DebugAttack2)
        {
            Debug.Log("<color=yellow>[Boss]</color> Attack Two Start");
            AttackPatternTwo();
        }
        else if (_state == BossState.DebugAttack3)
        {
            Debug.Log("<color=yellow>[Boss]</color> Attack Three Start");
            AttackPatternThree();
        }
        else
        {
            int currentIndex = Random.Range(0, 3);

            //�U���p�^�[�����A�����Ȃ��悤�ɍĒ��I���s���B
            while (currentIndex == _lastIndexOfAttack)
            {
                currentIndex = Random.Range(0, 3);
            }
            _lastIndexOfAttack = currentIndex;

            if (currentIndex == 0)
            {
                Debug.Log("<color=yellow>[Boss]</color> Attack One Start");
                AttackPatternOne();
            }
            else if (currentIndex == 1)
            {
                Debug.Log("<color=yellow>[Boss]</color> Attack Two Start");
                AttackPatternTwo();
            }
            else if (currentIndex == 2)
            {
                Debug.Log("<color=yellow>[Boss]</color> Attack Three Start");
                AttackPatternThree();
            }
            else
            {
                Debug.LogWarning("Attack pattern index was out of range!");
                ChooseAttack();
            }
        }
    }
    /// <summary>
    /// �U����E�U���O�̜p�j�ړ�
    /// </summary>
    public void WanderingMove()
    {
        _tweens.Add(
            this.transform.DOMove(new Vector2(-_startPos.x, _startPos.y), 3).
            SetLoops(2, LoopType.Yoyo).
            SetEase(Ease.InOutQuad).
            OnComplete(() =>
            {
                _bossCube.transform.DOPause();
                ChooseAttack();
            })
        );
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
    public virtual void FinalAttack()
    {
        Debug.LogError("<color=yellow>[BossBase]</color> Final attack isn't implemented!");
    }
    public abstract void PhaseSecondStart();
    /// <summary>
    /// ���S���̉��o
    /// </summary>
    public void Death()
    {
        const float duration = 4f;
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        FindObjectOfType<GameManager>().IsTimeStop = true;
        _seAus.PlayOneShot(_deathChargeSE);
        _seq?.Kill();
        transform.DOKill();
        _bossCube.transform.DOKill();
        this.transform.position = new Vector2(0, _startPos.y);
        Destroy(_particleTr.gameObject);
        _flashEffector.Flash();
        _tweens.Add(
            _bossCube.transform.DORotate(360 * Random.Range(4.6f, 5.1f) * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => StartCoroutine(Explode()))
        );
        StartCoroutine(FindObjectOfType<LightRay>().EmitLightRay(duration, 10));
    }
    /// <summary>
    /// �����G�t�F�N�g�֘A�B
    /// </summary>
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<CameraShaker>().Shake(3, 0, 1, 0.4f);
        _flashEffector.Flash();
        _bossCube.SetActive(false);
        FindObjectOfType<LightRay>().gameObject.SetActive(false);
        _seAus.PlayOneShot(_deathExplodeSE);
        Instantiate(_explodePrefab, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.5f);
        FindObjectOfType<FadeInOut>().FadeInAndChangeScene("StageClear");
    }
    public enum BossState
    {
        Normal,
        DebugAttack1,
        DebugAttack2,
        DebugAttack3,
        StartFinalAttackAtBeginning,
    }
    public void OnDisable()
    {
        _seq?.Kill();
        foreach (var t in _tweens)
        {
            t.Kill();
        }
    }

    public abstract void Pause();
    public abstract void Resume();
}
