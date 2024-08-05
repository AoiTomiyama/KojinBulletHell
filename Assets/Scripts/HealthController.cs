using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// �̗͂𐧌䂷��X�N���v�g
/// </summary>
public class HealthController : MonoBehaviour
{
    [Header("HP�̗�(-1�ő̗́�)")]
    [SerializeField]
    private int _health = 5;
    [SerializeField]
    private int _healthAtExpert = 5;
    [SerializeField]
    private int _healthAtRuthless = 5;

    [Header("�̗̓Q�[�W1�����̕�")]
    [SerializeField]
    private int _healthPadding = 5;

    [Header("�̗̓Q�[�W�̌�����")]
    [SerializeField]
    private GameObject _healthPrefab;

    [Header("�_���[�W����SE")]
    [SerializeField]
    private AudioClip _damageSE;
    /// <summary> ��ʏ�̗̑̓Q�[�W���Ǘ�����z�� </summary>
    private GameObject[] _healthBar;
    /// <summary> �����ƂȂ�AudioSource���擾 </summary>
    private AudioSource _aus;
    /// <summary> SE���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    private float _seVolume;

    private void Start()
    {
        _seVolume = PlayerPrefs.GetFloat("SEVolume");
        _aus = GetComponent<AudioSource>();
        if (_health != -1)
        {
            if (PlayerPrefs.GetString("DIFF") == "expert")
            {
                _health = _healthAtExpert;
            }
            if (PlayerPrefs.GetString("DIFF") == "ruthless")
            {
                _health = _healthAtRuthless;
            }
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
            if (_health - damage < 0)
            {
                Gameover();
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
                CameraShaker.Instance.Shake(1f, 0, 0.3f, 0);
                Debug.Log($"Bullet Hit! Took {damage} damage! Remaining health is {_health} !");
            }
            if (_health == 0)
            {
                Gameover();
            }
        }
        else
        {
            Debug.Log("Bullet Hit!");
        }
    }
    void Gameover()
    {
        Debug.Log("Gameover");
        SceneManager.LoadScene("Gameover");
    }
}
