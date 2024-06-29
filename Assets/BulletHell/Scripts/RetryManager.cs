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
        /// <summary>GameManagerがメイン画面時に記録していたシーン名をPlayerPrefsから持ってくる</summary>
        _oneBeforeSceneName = PlayerPrefs.GetString("Scene");
        _timeRecordTMPro.text = "Time: " + GameManager.GameTime.ToString("F2");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(_oneBeforeSceneName);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
