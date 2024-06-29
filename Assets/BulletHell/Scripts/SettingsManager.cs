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
    float _bgmVolume;
    /// <summary> SE音量の値を一時的に入れる変数 </summary>
    float _seVolume;
    /// <summary> 音源となるAudioSourceを取得 </summary>
    AudioSource _aus;
    private void Start()
    {
        _aus = GameObject.Find("BGM").GetComponent<AudioSource>();
        _settingsPanel.gameObject.SetActive(false);

        //スライダーの値をPlayerPrefsに保存されている値に応じてあらかじめ動かす。
        if (PlayerPrefs.HasKey("BGMVolume") && PlayerPrefs.HasKey("SEVolume"))
        {
            _sliders[0].value = PlayerPrefs.GetFloat("BGMVolume") * 10;
            _sliders[1].value = PlayerPrefs.GetFloat("SEVolume") * 10;
        }
        else
        {
            _sliders[0].value = 10f;
            _sliders[1].value = 10f;
        }
    }

    private void Update()
    {
        if (_settingsPanel.gameObject.activeSelf == true)
        {
            _bgmVolume = _aus.volume = (float)_sliders[0].value / 10;
            _seVolume = (float)_sliders[1].value / 10;

            _settingsText[0].text = "BGM: " + _sliders[0].value.ToString("F0");
            _settingsText[1].text = "SE: " + _sliders[1].value.ToString("F0");
        }
    }
    public void OnSettingsButtonClicked()
    {
        _settingsPanel.gameObject.SetActive(true);
    }
    
    public void OnCancelButtonClicked()
    {
        _settingsPanel.gameObject.SetActive(false);
        //キャンセルボタンが押されたとき（プレイヤーが設定を終えた時）にPlayerPrefsで値を保存している。
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetFloat("SEVolume", _seVolume);
        PlayerPrefs.Save();
    }

    public void OnSETesterButtonClicked()
    {
        var seSource = GameObject.Find("SE").GetComponent<AudioSource>();
        seSource.volume = _seVolume;
        seSource.Play();
    }
}
