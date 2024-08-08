using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �G�̗͂̊Ǘ��B
/// </summary>
public class EnemyHealthController : MonoBehaviour
{
    [SerializeField, Header("�G�̍ő�̗�")]
    private float _maxHealth = 30f;

    [SerializeField, Header("�G�q�b�g����SE")]
    private AudioClip _hitAtEnemySE;

    [SerializeField, Header("��������ǂꂾ��Y�������炷��")]
    private float _offsetFromCenter = 6f;

    /// <summary>���݂̗̑͂�ۑ�</summary>
    private float _health;
    /// <summary>�Ǐ]����{�X��GameObject</summary>
    private BossBase _boss;
    /// <summary>�̗͂�\������X���C�_�[</summary>
    private Slider _healthSlider;
    /// <summary>�̗͂�\������X���C�_�[</summary>
    private TextMeshProUGUI _healthText;
    /// <summary>SE��Audiosource���擾</summary>
    private AudioSource _seAus;
    /// <summary>���`�Ԃɓ˓��������ǂ���</summary>
    private bool _isPhaseSecondStarted;
    private void Start()
    {
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _boss = FindObjectOfType<BossBase>();
        _healthSlider = GetComponent<Slider>();
        var difficulty = PlayerPrefs.GetString("DIFF");
        if (difficulty == "expert")
        {
            _maxHealth *= 1.4f;
        }
        else if (difficulty == "ruthless")
        {
            _maxHealth *= 1.8f;
        }
        _health = _maxHealth;
        _healthText = transform.Find("EnemyHealthText").GetComponent<TextMeshProUGUI>();
        _healthSlider.value = _health / _maxHealth;
        _healthText.text = $"{_maxHealth}/{_health}";
    }
    private void FixedUpdate()
    {
        this.transform.position = Camera.main.WorldToScreenPoint(_boss.transform.position + Vector3.up * _offsetFromCenter);
    }
    public void EnemyDamage(int damage)
    {
        if (_health - damage == 0)
        {
            _boss.Death();
            Destroy(this.gameObject);
        }
        else
        {
            _seAus.PlayOneShot(_hitAtEnemySE);
            _health -= damage;
            _healthSlider.value = _health / _maxHealth;
            _healthText.text = $"{_maxHealth}/{_health}";
            if (_health <= _maxHealth / 2 && _isPhaseSecondStarted == false)
            {
                _boss.PhaseSecondStart();
                _isPhaseSecondStarted = true;
            }
        }
    }
}
