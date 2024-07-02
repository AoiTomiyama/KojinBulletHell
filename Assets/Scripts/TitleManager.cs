using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面の管理を行う。
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("クリック時のSE")]
    [SerializeField]
    AudioClip _pressedSE;
    [Header("キャンセル時のSE")]
    [SerializeField]
    AudioClip _cancelledSE;
    [Header("ここにMainMenuPanelを入れる")]
    [SerializeField]
    GameObject _mainMenuPanel;
    /// <summary>選択されたステージ名を取得</summary>
    string _selectedLevel;
    /// <summary>現在アクティブなパネルを保存</summary>
    GameObject _currentActivePanel;
    /// <summary>SEのAudiosourceを取得</summary>
    AudioSource _seAus;
    private void Start()
    {
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
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
        _seAus.PlayOneShot(_pressedSE);
    }
    public void PanelCancel(GameObject panel)
    {
        _currentActivePanel.SetActive(false);
        _currentActivePanel = panel;
        panel.SetActive(true);
        _seAus.PlayOneShot(_cancelledSE);
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
