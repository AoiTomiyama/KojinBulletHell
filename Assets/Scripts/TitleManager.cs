using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
