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
    private bool _isTimeStop;
    public bool IsTimeStop { set { _isTimeStop = value; } }
    private void Start()
    {
        //BGMとSEを設定画面で決めた値にする。
        var bgm = GameObject.Find("BGM");
        if (bgm != null)
        {
            bgm.GetComponent<AudioSource>().volume *= PlayerPrefs.GetFloat("BGMVolume");
        }
        var se = GameObject.Find("SE");
        if (se != null)
        {
            se.GetComponent<AudioSource>().volume *= PlayerPrefs.GetFloat("SEVolume");
        }

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
        if (!_isTimeStop)
        {
            _time += Time.deltaTime;
        }
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
