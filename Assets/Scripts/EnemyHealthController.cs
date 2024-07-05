using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �G�̗͂̊Ǘ��B
/// </summary>
public class EnemyHealthController : MonoBehaviour
{
    [Header("�G�̍ő�̗�")]
    [SerializeField]
    private float _maxHealth = 30f;

    [Header("�G�q�b�g����SE")]
    [SerializeField]
    private AudioClip _hitAtEnemySE;

    /// <summary>���݂̗̑͂�ۑ�</summary>
    private float _health;
    /// <summary>�Ǐ]����{�X��GameObject</summary>
    private GameObject _followTarget;
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
        _followTarget = FindObjectOfType<BossABehaviour>().gameObject;
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
        float offsetFromCenter = 6f;
        this.transform.position = Camera.main.WorldToScreenPoint(_followTarget.transform.position + Vector3.up * offsetFromCenter);
    }
    public void EnemyDamage(int damage)
    {
        if (_health - damage == 0)
        {
            _followTarget.GetComponent<BossABehaviour>().Death();
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
                _followTarget.GetComponent<BossABehaviour>().PhaseSecondStart();
                _isPhaseSecondStarted = true;
            }
        }
    }
}
