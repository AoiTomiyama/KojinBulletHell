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
    int _health = 5; 

    [Header("体力ゲージ1つおきの幅")]
    [SerializeField]
    int _healthPadding = 5;

    [Header("体力ゲージの見た目")]
    [SerializeField]
    GameObject _healthPrefab;

    /// <summary> 画面上の体力ゲージを管理する配列 </summary>
    GameObject[] _healthBar;

    private void Start()
    {
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
            if (_health - damage < 0)
            {
                Gameover();
                return;
            }
            else
            {
                for (int i = _health; i > _health - damage; i--)
                {
                    Destroy(_healthBar[i - 1]);
                }
                _health -= damage;
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
