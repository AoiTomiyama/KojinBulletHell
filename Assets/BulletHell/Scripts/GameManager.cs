using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム画面自体を管理するスクリプト
/// 現在タイマーの管理のみ
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary> 経過時間。 </summary>
    private static float _time;
    public static float GameTime => _time;
    [Header("経過時間を表示させるテキスト")]
    [SerializeField]
    Text _timerText;
    private void Start()
    {
        GameObject.Find("BGM").GetComponent<AudioSource>().volume *= (float)PlayerPrefs.GetInt("BGMVolume") / 10;
        _time = 0f;
        _timerText = GameObject.Find("TimeField").GetComponent<Text>();
    }
    void FixedUpdate()
    {
        _time += Time.deltaTime;
        if (_timerText != null)
        {
            _timerText.text = _time.ToString("F2");
        }
    }
}
