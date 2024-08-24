using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// リトライ画面を管理するスクリプト
/// </summary>
public class RetryManager : MonoBehaviour
{
    [SerializeField, Header("時間の記録を出力するテキスト")]
    private TextMeshProUGUI _timeRecordText;

    [SerializeField, Header("選んだ難易度を出力するテキスト")]
    private TextMeshProUGUI _difficultyText;

    [SerializeField, Header("クリア表示")]
    private TextMeshProUGUI _clearText;

    [SerializeField, Header("上2つを使用するか")]
    private bool _isClearScene;

    /// <summary>直前のシーン名を入れる変数</summary>
    private string _oneBeforeSceneName;
    private void Start()
    {
        //GameManagerがメイン画面時に記録していたシーン名をPlayerPrefsから持ってくる
        _oneBeforeSceneName = PlayerPrefs.GetString("Scene");
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEVolume");
        _timeRecordText.text = "Time: " + PlayerPrefs.GetFloat("Time").ToString("F2");
        if (_isClearScene)
        {
            _clearText.text = $"-{_oneBeforeSceneName}-\r\n-COMPLETE-";
            _difficultyText.text = "Difficulty: " + PlayerPrefs.GetString("DIFF").ToUpper();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FindObjectOfType<FadeInOut>().FadeInAndChangeScene(_oneBeforeSceneName);
            PlayerPrefs.DeleteKey("Time");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            FindObjectOfType<FadeInOut>().FadeInAndChangeScene("Title");
            PlayerPrefs.DeleteKey("Time");
        }
    }
}
