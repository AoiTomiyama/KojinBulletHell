using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// �^�C�g����ʂ̊Ǘ����s���B
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("�{�^���������ꂽ�Ƃ���SE")]
    [SerializeField]
    AudioClip _pressedSE;
    [Header("������LevelSelectPanel������")]
    [SerializeField]
    GameObject _levelSelectPanel;
    [Header("������MainMenuPanel������")]
    [SerializeField]
    GameObject _mainMenuPanel;
    [Header("������DifficultyPanel������")]
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
