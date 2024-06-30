using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        _levelSelectPanel.SetActive(false);
        PlayerPrefs.DeleteKey("Scene");
    }
    public void OnStartButtonClicked()
    {
        _mainMenuPanel.SetActive(false);
        _levelSelectPanel.SetActive(true);
    }
    public void OnBackToTitleButtonClicked()
    {
        _mainMenuPanel.SetActive(true);
        _levelSelectPanel.SetActive(false);
    }
    public void OnLevelButtonClicked(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
