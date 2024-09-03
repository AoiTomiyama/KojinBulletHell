using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// �̗͂𐧌䂷��X�N���v�g
/// </summary>
public class HealthController : MonoBehaviour
{
    [SerializeField, Header("HP�̗�(-1�ő̗́�)")]
    private int _health = 5;

    [SerializeField, Header("�̗̓Q�[�W1�����̕�")]
    private int _healthPadding = 5;

    [SerializeField, Header("�̗̓Q�[�W�̌�����")]
    private GameObject _healthPrefab;

    [SerializeField, Header("�_���[�W����SE")]
    private AudioClip _damageSE;

    [SerializeField, Header("���S����SE")]
    private AudioClip _deathSE;

    /// <summary> ��ʏ�̗̑̓Q�[�W���Ǘ�����z�� </summary>
    private GameObject[] _healthBar;
    /// <summary> �����ƂȂ�AudioSource���擾 </summary>
    private AudioSource _aus;
    /// <summary> SE���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    private float _seVolume;
    /// <summary>���@�̗̑͂��[���ɂȂ������Ɏ��s����f���Q�[�g</summary>
    public event Action OnGameOver;

    private void Start()
    {
        _seVolume = PlayerPrefs.GetFloat("SEVolume");
        _aus = GetComponent<AudioSource>();
        if (_health != -1)
        {
            _healthBar = new GameObject[_health];
            for (int i = 0; i < _health; i++)
            {
                _healthBar[i] = Instantiate(_healthPrefab, transform);
                _healthBar[i].transform.position = new Vector2(this.transform.position.x + i * Screen.width / _healthPadding, this.transform.position.y);
            }
        }
        else
        {
            GetComponent<Text>().text = "��";
        }
    }
    public void RemoveHealth(int damage)
    {
        if (_health != -1)
        {
            if (_health - damage <= 0)
            {
                StartCoroutine(Gameover());
                return;
            }
            else
            {
                for (int i = _health; i > _health - damage; i--)
                {
                    _healthBar[i - 1].GetComponent<Image>().color = Color.red;
                    Destroy(_healthBar[i - 1], 0.5f);
                }
                _health -= damage;
                _aus.PlayOneShot(_damageSE, _aus.volume * _seVolume);
                FindObjectOfType<CameraShaker>().Shake(1f, 0, 0.3f, 0);
                Debug.Log($"<color=purple>[PlayerHealth]</color> Bullet Hit! Took {damage} damage! Remaining health is {_health} !");
            }
        }
        else
        {
            Debug.Log("<color=purple>[PlayerHealth][DEBUG]</color> Bullet Hit!");
        }
    }
    private IEnumerator Gameover()
    {
        Array.ForEach(_healthBar, Destroy);
        Debug.Log("<color=purple>[PlayerHealth]</color> Gameover");
        _aus.PlayOneShot(_deathSE);
        GameObject.Find("BGM").GetComponent<AudioSource>().DOPitch(0, 1.5f);
        OnGameOver();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Gameover");
    }
}
