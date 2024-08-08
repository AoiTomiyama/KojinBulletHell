using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �^�C�g����ʂ̊Ǘ����s���B
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("�N���b�N����SE")]
    private AudioClip _pressedSE;

    [SerializeField, Header("�L�����Z������SE")]
    private AudioClip _cancelledSE;

    [SerializeField, Header("������MainMenuPanel������")]
    private GameObject _mainMenuPanel;

    /// <summary>�I�����ꂽ�X�e�[�W�����擾</summary>
    private string _selectedLevel;
    /// <summary>���݃A�N�e�B�u�ȃp�l����ۑ�</summary>
    private GameObject _currentActivePanel;
    /// <summary>SE��Audiosource���擾</summary>
    private AudioSource _seAus;
    private void Start()
    {
        _seAus = GameObject.Find("SE").GetComponent<AudioSource>();
        _mainMenuPanel.SetActive(true);
        _currentActivePanel = _mainMenuPanel;
        //�^�C�g���ɖ߂��Ă����Ƃ��ɁA�s�v��PlayerPrefs��j��
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
