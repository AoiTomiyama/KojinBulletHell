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
    float _bgmVolume;
    /// <summary> SE���ʂ̒l���ꎞ�I�ɓ����ϐ� </summary>
    float _seVolume;
    /// <summary> �����ƂȂ�AudioSource���擾 </summary>
    AudioSource _aus;
    private void Start()
    {
        _aus = GameObject.Find("BGM").GetComponent<AudioSource>();
        _settingsPanel.gameObject.SetActive(false);

        //�X���C�_�[�̒l��PlayerPrefs�ɕۑ�����Ă���l�ɉ����Ă��炩���ߓ������B
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
        //�L�����Z���{�^���������ꂽ�Ƃ��i�v���C���[���ݒ���I�������j��PlayerPrefs�Œl��ۑ����Ă���B
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
