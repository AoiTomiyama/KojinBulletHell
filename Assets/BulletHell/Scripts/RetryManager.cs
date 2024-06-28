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
    private void Start()
    {
        _timeRecordTMPro.text = "Time: " + GameManager.GameTime.ToString("F2");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("BulletHell");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
