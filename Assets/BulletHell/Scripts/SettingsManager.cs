using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音量の設定をPlayerPrefsに保存させるスクリプト
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("ここに設定パネルを入れる")]
    [SerializeField]
    Image _settingsPanel;

    [Header("ここにスライダーを入れる")]
    [SerializeField]
    Slider[] _sliders;

    [Header("ここにスライダーの値を表示するテキストフィールドを入れる")]
    [SerializeField]
    TextMeshProUGUI[] _settingsText;
    /// <summary> BGM音量の値を一時的に入れる変数 </summary>
    int _bgmVolume;
    /// <summary> SE音量の値を一時的に入れる変数 </summary>
    int _seVolume;
    private void Start()
    {
        _settingsPanel.gameObject.SetActive(false);

        //スライダーの値をPlayerPrefsに保存されている値に応じてあらかじめ動かす。
        if (PlayerPrefs.HasKey("BGMVolume") && PlayerPrefs.HasKey("SEVolume"))
        {
            _sliders[0].value = (float)PlayerPrefs.GetInt("BGMVolume") / 10;
            _sliders[1].value = (float)PlayerPrefs.GetInt("SEVolume") / 10;
        }
    }

    private void FixedUpdate()
    {
        _bgmVolume = Mathf.CeilToInt(_sliders[0].value * 10);
        _seVolume = Mathf.CeilToInt(_sliders[1].value * 10); 
        
        _settingsText[0].text = "BGM: " + _bgmVolume.ToString("F0");
        _settingsText[1].text = "SE: " + _seVolume.ToString("F0");
    }
    public void OnSettingsButtonClicked()
    {
        _settingsPanel.gameObject.SetActive(true);
    }
    
    public void OnCancelButtonClicked()
    {
        _settingsPanel.gameObject.SetActive(false);
        //キャンセルボタンが押されたとき（プレイヤーが設定を終えた時）にPlayerPrefsで値を保存している。
        PlayerPrefs.SetInt("BGMVolume", _bgmVolume);
        PlayerPrefs.SetInt("SEVolume", _seVolume);
        PlayerPrefs.Save();
    }
}
