using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �Q�[����ʎ��̂��Ǘ�����X�N���v�g
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary> �o�ߎ��ԁB </summary>
    private float _time;
    /// <summary> �o�ߎ��Ԃ�\��������e�L�X�g</summary>
    private Text _timerText;
    /// <summary> �{�X���j���Ƀ^�C�}�[���~�߂�p</summary>
    private bool _isTimeStop;
    public bool IsTimeStop { set { _isTimeStop = value; } }
    private void Start()
    {
        //BGM��SE��ݒ��ʂŌ��߂��l�ɂ���B
        var bgm = GameObject.Find("BGM");
        if (bgm != null)
        {
            bgm.GetComponent<AudioSource>().volume *= PlayerPrefs.GetFloat("BGMVolume");
        }
        var se = GameObject.Find("SE");
        if (se != null)
        {
            se.GetComponent<AudioSource>().volume *= PlayerPrefs.GetFloat("SEVolume");
        }

        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        _time = 0f;
        _timerText = GameObject.Find("TimeField").GetComponent<Text>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene("Title");
        }
    }
    void FixedUpdate()
    {
        if (!_isTimeStop)
        {
            _time += Time.deltaTime;
            if (_timerText != null)
            {
                _timerText.text = _time.ToString("000.00");
            }
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Time", _time);
        PlayerPrefs.Save();
    }
}
