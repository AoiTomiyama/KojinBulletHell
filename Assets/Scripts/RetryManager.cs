using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���g���C��ʂ��Ǘ�����X�N���v�g
/// </summary>
public class RetryManager : MonoBehaviour
{
    [Header("�L�^���o�͂���e�L�X�g")]
    [SerializeField]
    TextMeshProUGUI _timeRecordTMPro;
    /// <summary>���O�̃V�[����������ϐ�</summary>
    string _oneBeforeSceneName;
    private void Start()
    {
        //GameManager�����C����ʎ��ɋL�^���Ă����V�[������PlayerPrefs���玝���Ă���
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
