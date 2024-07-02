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
    /// <summary>���݂̗̑͂�ۑ�</summary>
    private float _health;
    /// <summary>�Ǐ]����{�X��GameObject</summary>
    GameObject _followTarget;
    /// <summary>�̗͂�\������X���C�_�[</summary>
    Slider _healthSlider;
    private void Start()
    {
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
            _health -= damage;
            _healthSlider.value = _health / _maxHealth;
        }
    }
}
