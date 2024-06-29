using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// リトライ画面を管理するスクリプト
/// </summary>
public class RetryManager : MonoBehaviour
{
    [Header("記録を出力するテキスト")]
    [SerializeField]
    TextMeshProUGUI _timeRecordTMPro;
    /// <summary>直前のシーン名を入れる変数</summary>
    string _oneBeforeSceneName;
    private void Start()
    {
        //GameManagerがメイン画面時に記録していたシーン名をPlayerPrefsから持ってくる
        _oneBeforeSceneName = PlayerPrefs.GetString("Scene");
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEVolume");
        _timeRecordTMPro.text = "time: " + PlayerPrefs.GetFloat("Time").ToString("F2");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(_oneBeforeSceneName);
            PlayerPrefs.DeleteKey("Time");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Title");
            PlayerPrefs.DeleteKey("Time");
        }
    }
}
