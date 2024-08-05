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

    [Header("設定パネルを開くときのSE")]
    [SerializeField]
    AudioClip _openSettingsSE;

    [Header("設定パネルを閉じるときのSE")]
    [SerializeField]
    AudioClip _closeSettingsSE;

    /// <summary> BGM音量の値を一時的に入れる変数 </summary>
    float _bgmVolume;
    /// <summary> SE音量の値を一時的に入れる変数 </summary>
    float _seVolume;
    /// <summary> BGM音源となるAudioSourceを取得 </summary>
    AudioSource _bgmAus;
    /// <summary> SE音源となるAudioSourceを取得 </summary>
    AudioSource _seAus;
    private void Start()
    {
        _bgmAus = GameObject.Find("BGM").GetComponent<AudioSource>();
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _settingsPanel.gameObject.SetActive(false);

        //スライダーの値をPlayerPrefsに保存されている値に応じてあらかじめ動かす。
        if (PlayerPrefs.HasKey("BGMVolume") && PlayerPrefs.HasKey("SEVolume"))
        {
            _sliders[0].value = PlayerPrefs.GetFloat("BGMVolume") * 10;
            _sliders[1].value = PlayerPrefs.GetFloat("SEVolume") * 10;
            _bgmAus.volume = PlayerPrefs.GetFloat("BGMVolume");
            _seAus.volume = PlayerPrefs.GetFloat("BGMVolume");
        }
        else
        {
            PlayerPrefs.SetFloat("BGMVolume", 1f);
            PlayerPrefs.SetFloat("SEVolume", 1f);
            PlayerPrefs.Save();
            _sliders[0].value = 10f;
            _sliders[1].value = 10f;
        }
    }

    private void Update()
    {
        if (_settingsPanel.gameObject.activeSelf == true)
        {
            _bgmVolume = _bgmAus.volume = (float)_sliders[0].value / 10;
            _seVolume = _seAus.volume = (float)_sliders[1].value / 10;

            _settingsText[0].text = "BGM: " + _sliders[0].value.ToString("F0");
            _settingsText[1].text = "SE: " + _sliders[1].value.ToString("F0");
        }
    }
    public void OnSettingsButtonClicked()
    {
        _seAus.PlayOneShot(_openSettingsSE);
        _settingsPanel.gameObject.SetActive(true);
    }
    
    public void OnCancelButtonClicked()
    {
        _seAus.PlayOneShot(_closeSettingsSE);
        _settingsPanel.gameObject.SetActive(false);
        //キャンセルボタンが押されたとき（プレイヤーが設定を終えた時）にPlayerPrefsで値を保存している。
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetFloat("SEVolume", _seVolume);
        PlayerPrefs.Save();
    }
}
