using TMPro;
using UnityEngine;

/// <summary>
/// リトライ画面を管理するスクリプト
/// </summary>
public class ClearSceneManager : MonoBehaviour
{
    [SerializeField, Header("時間の記録を出力するテキスト")]
    private TextMeshProUGUI _timeRecordText;

    [SerializeField, Header("選んだ難易度を出力するテキスト")]
    private TextMeshProUGUI _difficultyText;

    [SerializeField, Header("クリア表示")]
    private TextMeshProUGUI _clearText;

    /// <summary>直前のシーン名を入れる変数</summary>
    private string _oneBeforeSceneName;

    private void Start()
    {
        //GameManagerがメイン画面時に記録していたシーン名をPlayerPrefsから持ってくる
        _oneBeforeSceneName = PlayerPrefs.GetString("Scene");
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEVolume");

        var record = PlayerPrefs.GetFloat("Time");
        if (PlayerPrefs.HasKey("DIFF_INT") && _oneBeforeSceneName != "")
        {
            SaveDateManager.Instance.AddData(new SaveDateManager.Record(_oneBeforeSceneName, PlayerPrefs.GetInt("DIFF_INT"), record));
        }

        _timeRecordText.text = "Time: " + record.ToString("F2");

        _clearText.text = $"-{_oneBeforeSceneName}-\r\n-COMPLETE-";
        _difficultyText.text = "Difficulty: " + PlayerPrefs.GetString("DIFF").ToUpper();
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
