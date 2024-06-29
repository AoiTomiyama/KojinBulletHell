using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ʂ̐ݒ��PlayerPrefs�ɕۑ�������X�N���v�g
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("�����ɐݒ�p�l��������")]
    [SerializeField]
    Image _settingsPanel;

    [Header("�����ɃX���C�_�[������")]
    [SerializeField]
    Slider[] _sliders;

    [Header("�����ɃX���C�_�[�̒l��\������e�L�X�g�t�B�[���h������")]
    [SerializeField]
    TextMeshProUGUI[] _settingsText;
    /// <summary> BGM���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    int _bgmVolume;
    /// <summary> SE���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    int _seVolume;
    private void Start()
    {
        _settingsPanel.gameObject.SetActive(false);

        //�X���C�_�[�̒l��PlayerPrefs�ɕۑ�����Ă���l�ɉ����Ă��炩���ߓ������B
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
        //�L�����Z���{�^���������ꂽ�Ƃ��i�v���C���[���ݒ���I�������j��PlayerPrefs�Œl��ۑ����Ă���B
        PlayerPrefs.SetInt("BGMVolume", _bgmVolume);
        PlayerPrefs.SetInt("SEVolume", _seVolume);
        PlayerPrefs.Save();
    }
}
