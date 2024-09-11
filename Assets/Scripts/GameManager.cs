using Cinemachine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲーム画面自体を管理するスクリプト
/// </summary>
public class GameManager : MonoBehaviour, IPausable
{
    /// <summary> 経過時間。 </summary>
    private float _time;
    /// <summary> 経過時間を表示させるテキスト</summary>
    private Text _timerText;
    /// <summary> ボス撃破時にタイマーを止める用</summary>
    private bool _isTimeStop;
    public bool IsTimeStop { set => _isTimeStop = value; }
    private void Start()
    {
        //カメラの振動を初期化。
        CinemachineImpulseManager.Instance.Clear();

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
        _timerText.text = _time.ToString("000.00");
    }
    private void Update()
    {
        if (!_isTimeStop)
        {
            _time += Time.deltaTime;
            if (_timerText != null)
            {
                _timerText.text = _time.ToString("000.00");
            }
        }
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Time", _time);
        PlayerPrefs.Save();
    }

    public void Pause()
    {
        _isTimeStop = true;
    }

    public void Resume()
    {
        _isTimeStop = false;
    }
}
public static class Enums
{
    public enum Difficulties
    {
        Normal,
        Expert,
        Ruthless
    }
}