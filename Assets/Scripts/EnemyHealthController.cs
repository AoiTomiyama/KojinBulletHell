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
    private HealthStatus _healthStatus = HealthStatus.Normal;
    /// <summary>��Փx</summary>
    private Enums.Difficulties _difficulty;
    private void Start()
    {
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _boss = FindObjectOfType<BossBase>();
        _healthSlider = GetComponent<Slider>();
        _difficulty = (Enums.Difficulties)PlayerPrefs.GetInt("DIFF_INT");
        if (_difficulty == Enums.Difficulties.Expert)
        {
            _maxHealth *= 1.5f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            _maxHealth *= 2f;
        }
        _health = _maxHealth;
        _healthText = transform.Find("EnemyHealthText").GetComponent<TextMeshProUGUI>();
        _healthSlider.value = _health / _maxHealth;
        _healthText.text = $"{_maxHealth}/{_health}";
    }
    private void FixedUpdate()
    {
        if (_boss != null)
        {
            this.transform.position = Camera.main.WorldToScreenPoint(_boss.transform.position + Vector3.up * _offsetFromCenter);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void EnemyDamage(int damage)
    {
        if (_health - damage == 0)
        {
            //���S���o���̓|�[�Y���ł��Ȃ��悤�ɂ���B
            FindObjectOfType<PauseManager>().EnablePause = false;

            _seAus.PlayOneShot(_hitAtEnemySE);
            _boss.Death();
            Destroy(this.gameObject);
        }
        else
        {
            _seAus.PlayOneShot(_hitAtEnemySE);
            _health -= damage;
            _healthSlider.value = _health / _maxHealth;
            _healthText.text = $"{_maxHealth}/{_health}";
            if (_health <= _maxHealth / 2 && _healthStatus == HealthStatus.Normal)
            {
                Debug.Log("<color=yellow>[Boss]</color> Phase 2 Start");
                _boss.PhaseSecondStart();
                _healthStatus = HealthStatus.Half;
            }
            else if (_health == 1 && _healthStatus == HealthStatus.Half && _difficulty != Enums.Difficulties.Normal)
            {
                Debug.Log("<color=yellow>[Boss]</color> Final Attack Start");
                _boss.FinalAttack();
                _healthStatus = HealthStatus.Last;
            }
        }
    }
    public enum HealthStatus
    {
        Normal,
        Half,
        Last
    }
}
