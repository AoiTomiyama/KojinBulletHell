using UnityEngine;
using UnityEngine.UI;

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
