using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 体力を制御するスクリプト
/// </summary>
public class HealthController : MonoBehaviour
{
    [Header("HPの量(-1で体力∞)")]
    [SerializeField]
    private int _health = 5;
    [SerializeField]
    private int _healthAtExpert = 5;
    [SerializeField]
    private int _healthAtRuthless = 5;

    [Header("体力ゲージ1つおきの幅")]
    [SerializeField]
    private int _healthPadding = 5;

    [Header("体力ゲージの見た目")]
    [SerializeField]
    private GameObject _healthPrefab;

    [Header("ダメージ時のSE")]
    [SerializeField]
    private AudioClip _damageSE;
    /// <summary> 画面上の体力ゲージを管理する配列 </summary>
    private GameObject[] _healthBar;
    /// <summary> 音源となるAudioSourceを取得 </summary>
    private AudioSource _aus;
    /// <summary> SE音量の値を一時的に入れる変数 </summary>
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
            GetComponent<Text>().text = "∞";
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
