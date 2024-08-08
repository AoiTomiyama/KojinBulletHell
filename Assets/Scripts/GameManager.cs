using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲーム画面自体を管理するスクリプト
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary> 経過時間。 </summary>
    private float _time;
    /// <summary> 経過時間を表示させるテキスト</summary>
    private Text _timerText;
    /// <summary> ボス撃破時にタイマーを止める用</summary>
    private bool _isTimeStop;
    public bool IsTimeStop { set { _isTimeStop = value; } }
    private void Start()
    {
        //BGMとSEを設定画面で決めた値にする。
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
