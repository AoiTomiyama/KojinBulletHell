using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面の管理を行う。
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("ボタンが押されたときのSE")]
    [SerializeField]
    AudioClip _pressedSE;
    [Header("ここにMainMenuPanelを入れる")]
    [SerializeField]
    GameObject _mainMenuPanel;
    /// <summary>選択されたステージ名を取得</summary>
    string _selectedLevel;
    /// <summary>現在アクティブなパネルを保存</summary>
    GameObject _currentActivePanel;
    private void Start()
    {
        _mainMenuPanel.SetActive(true);
        _currentActivePanel = _mainMenuPanel;
        //タイトルに戻ってきたときに、不要なPlayerPrefsを破棄
        PlayerPrefs.DeleteKey("Scene");
        PlayerPrefs.DeleteKey("DIFF");
    }
    public void PanelMove(GameObject panel)
    {
        _currentActivePanel.SetActive(false);
        _currentActivePanel = panel;
        panel.SetActive(true);
    }
    public void OnTutorialButtonClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void OnDifficultyButtonClicked(string difficulty)
    {
        PlayerPrefs.SetString("DIFF", difficulty);
        PlayerPrefs.Save();
        SceneManager.LoadScene(_selectedLevel);
    }
    public void OnSelectLevelClicked(string selectedLevel)
    {
        _selectedLevel = selectedLevel;
    }
}
