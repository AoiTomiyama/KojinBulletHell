using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵体力の管理。
/// </summary>
public class EnemyHealthController : MonoBehaviour
{
    [SerializeField, Header("敵の最大体力")]
    private float _maxHealth = 30f;

    [SerializeField, Header("敵ヒット時のSE")]
    private AudioClip _hitAtEnemySE;

    [SerializeField, Header("中央からどれだけY軸をずらすか")]
    private float _offsetFromCenter = 6f;

    /// <summary>現在の体力を保存</summary>
    private float _health;
    /// <summary>追従するボスのGameObject</summary>
    private BossBase _boss;
    /// <summary>体力を表示するスライダー</summary>
    private Slider _healthSlider;
    /// <summary>体力を表示するスライダー</summary>
    private TextMeshProUGUI _healthText;
    /// <summary>SEのAudiosourceを取得</summary>
    private AudioSource _seAus;
    /// <summary>第二形態に突入したかどうか</summary>
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
