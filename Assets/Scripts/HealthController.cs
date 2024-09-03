using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 体力を制御するスクリプト
/// </summary>
public class HealthController : MonoBehaviour
{
    [SerializeField, Header("HPの量(-1で体力∞)")]
    private int _health = 5;

    [SerializeField, Header("体力ゲージ1つおきの幅")]
    private int _healthPadding = 5;

    [SerializeField, Header("体力ゲージの見た目")]
    private GameObject _healthPrefab;

    [SerializeField, Header("ダメージ時のSE")]
    private AudioClip _damageSE;

    [SerializeField, Header("死亡時のSE")]
    private AudioClip _deathSE;

    /// <summary> 画面上の体力ゲージを管理する配列 </summary>
    private GameObject[] _healthBar;
    /// <summary> 音源となるAudioSourceを取得 </summary>
    private AudioSource _aus;
    /// <summary> SE音量の値を一時的に入れる変数 </summary>
    private float _seVolume;
    /// <summary>自機の体力がゼロになった時に実行するデリゲート</summary>
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
            GetComponent<Text>().text = "∞";
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
