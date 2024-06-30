using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲーム画面自体を管理するスクリプト
/// 現在タイマーの管理のみ
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary> 経過時間。 </summary>
    private float _time;
    [Header("経過時間を表示させるテキスト")]
    [SerializeField]
    Text _timerText;
    private void Start()
    {
        GameObject.Find("BGM").GetComponent<AudioSource>().volume *= PlayerPrefs.GetFloat("BGMVolume");
        GameObject.Find("SE").GetComponent<AudioSource>().volume *= PlayerPrefs.GetFloat("SEVolume");
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        _time = 0f;
        _timerText = GameObject.Find("TimeField").GetComponent<Text>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene("Title");
        }
    }
    void FixedUpdate()
    {
        _time += Time.deltaTime;
        if (_timerText != null)
        {
            _timerText.text = _time.ToString("F2");
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Time", _time);
        PlayerPrefs.Save();
    }
}
