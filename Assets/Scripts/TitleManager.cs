using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトル画面の管理を行う。
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("クリック時のSE")]
    private AudioClip _pressedSE;

    [SerializeField, Header("キャンセル時のSE")]
    private AudioClip _cancelledSE;

    [SerializeField, Header("ここにMainMenuPanelを入れる")]
    private GameObject _mainMenuPanel;

    /// <summary>選択されたステージ名を取得</summary>
    private string _selectedLevel;
    /// <summary>現在アクティブなパネルを保存</summary>
    private GameObject _currentActivePanel;
    /// <summary>SEのAudiosourceを取得</summary>
    private AudioSource _seAus;
    private void Start()
    {
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _mainMenuPanel.SetActive(true);
        _currentActivePanel = _mainMenuPanel;
        //タイトルに戻ってきたときに、不要なPlayerPrefsを破棄
        PlayerPrefs.DeleteKey("Scene");
        PlayerPrefs.DeleteKey("DIFF");
        PlayerPrefs.DeleteKey("DIFF_INT");
    }
    public void PanelMove(GameObject panel)
    {
        _currentActivePanel.GetComponent<Animator>().Play("UIExit");
        _currentActivePanel = panel;
        panel.SetActive(true);
        _seAus.PlayOneShot(_pressedSE);
    }
    public void PanelCancel(GameObject panel)
    {
        _currentActivePanel.GetComponent<Animator>().Play("UIExit");
        _currentActivePanel = panel;
        panel.SetActive(true);
        panel.GetComponent<Animator>().Play("UIEnter");
        _seAus.PlayOneShot(_cancelledSE);
    }
    public void OnTutorialButtonClicked()
    {
        FindObjectOfType<FadeInOut>().FadeInAndChangeScene("Tutorial");
    }
    public void OnDifficultyButtonClicked(int difficulty)
    {
        PlayerPrefs.SetString("DIFF", ((Enums.Difficulties)difficulty).ToString());
        PlayerPrefs.SetInt("DIFF_INT", difficulty);
        PlayerPrefs.Save();
        FindObjectOfType<FadeInOut>().FadeInAndChangeScene(_selectedLevel);
    }
    public void OnSelectLevelClicked(string selectedLevel)
    {
        _selectedLevel = selectedLevel;
    }
}
