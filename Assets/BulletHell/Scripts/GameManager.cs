using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �Q�[����ʎ��̂��Ǘ�����X�N���v�g
/// ���݃^�C�}�[�̊Ǘ��̂�
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary> �o�ߎ��ԁB </summary>
    private static float _time;
    public static float GameTime => _time;
    [Header("�o�ߎ��Ԃ�\��������e�L�X�g")]
    [SerializeField]
    Text _timerText;
    private void Start()
    {
        GameObject.Find("BGM").GetComponent<AudioSource>().volume *= PlayerPrefs.GetFloat("BGMVolume");
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        _time = 0f;
        _timerText = GameObject.Find("TimeField").GetComponent<Text>();
    }
    void FixedUpdate()
    {
        _time += Time.deltaTime;
        if (_timerText != null)
        {
            _timerText.text = _time.ToString("F2");
        }
    }
}
