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
    [Header("ここにLevelSelectPanelを入れる")]
    [SerializeField]
    GameObject _levelSelectPanel;
    [Header("ここにMainMenuPanelを入れる")]
    [SerializeField]
    GameObject _mainMenuPanel;
    [Header("ここにDifficultyPanelを入れる")]
    [SerializeField]
    GameObject _difficultyPanel;
    string _selectedLevel;
    GameObject _currentActivePanel;
    private void Start()
    {
        _mainMenuPanel.SetActive(true);
        _currentActivePanel = _mainMenuPanel;
        _levelSelectPanel.SetActive(false);
        _difficultyPanel.SetActive(false);
        PlayerPrefs.DeleteKey("Scene");
    }
    public void PanelMove(GameObject panel)
    {
        _currentActivePanel.SetActive(false);
        _currentActivePanel = panel;
        panel.SetActive(true);
    }
    public void OnLevelButtonClicked(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void OnDifficultyButtonClicked(string difficulty)
    {
        PlayerPrefs.SetString("DIFF", difficulty);
        SceneManager.LoadScene(_selectedLevel);
    }
    public void OnSelectLevelClicked(string selectedLevel)
    {
        _selectedLevel = selectedLevel;
    }
}
