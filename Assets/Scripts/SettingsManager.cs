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

    [Header("�ݒ�p�l�����J���Ƃ���SE")]
    [SerializeField]
    AudioClip _openSettingsSE;

    [Header("�ݒ�p�l�������Ƃ���SE")]
    [SerializeField]
    AudioClip _closeSettingsSE;

    /// <summary> BGM���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    float _bgmVolume;
    /// <summary> SE���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    float _seVolume;
    /// <summary> BGM�����ƂȂ�AudioSource���擾 </summary>
    AudioSource _bgmAus;
    /// <summary> SE�����ƂȂ�AudioSource���擾 </summary>
    AudioSource _seAus;
    private void Start()
    {
        _bgmAus = GameObject.Find("BGM").GetComponent<AudioSource>();
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _settingsPanel.gameObject.SetActive(false);

        //�X���C�_�[�̒l��PlayerPrefs�ɕۑ�����Ă���l�ɉ����Ă��炩���ߓ������B
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
        //�L�����Z���{�^���������ꂽ�Ƃ��i�v���C���[���ݒ���I�������j��PlayerPrefs�Œl��ۑ����Ă���B
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetFloat("SEVolume", _seVolume);
        PlayerPrefs.Save();
    }
}
