using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵体力の管理。
/// </summary>
public class EnemyHealthController : MonoBehaviour
{
    [Header("敵の最大体力")]
    [SerializeField]
    private float _maxHealth = 30f;
    [Header("敵ヒット時のSE")]
    [SerializeField]
    AudioClip _hitAtEnemySE;
    /// <summary>現在の体力を保存</summary>
    private float _health;
    /// <summary>追従するボスのGameObject</summary>
    GameObject _followTarget;
    /// <summary>体力を表示するスライダー</summary>
    Slider _healthSlider;
    /// <summary>SEのAudiosourceを取得</summary>
    AudioSource _seAus;
    private void Start()
    {
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _followTarget = FindObjectOfType<BossABehaviour>().gameObject;
        _healthSlider = GetComponent<Slider>();
        _health = _maxHealth;
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
            if (_health == _maxHealth / 2)
            {
                _followTarget.GetComponent<BossABehaviour>().PhaseSecondStart();
            }
        }
    }
}
