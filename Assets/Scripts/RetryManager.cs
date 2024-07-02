using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���g���C��ʂ��Ǘ�����X�N���v�g
/// </summary>
public class RetryManager : MonoBehaviour
{
    [Header("���Ԃ̋L�^���o�͂���e�L�X�g")]
    [SerializeField]
    TextMeshProUGUI _timeRecordText;
    [Header("�I�񂾓�Փx���o�͂���e�L�X�g")]
    [SerializeField]
    TextMeshProUGUI _difficultyText;
    [Header("�N���A�\��")]
    [SerializeField]
    TextMeshProUGUI _clearText;
    [Header("��2���g�p���邩")]
    [SerializeField]
    bool _isClearScene;
    /// <summary>���O�̃V�[����������ϐ�</summary>
    string _oneBeforeSceneName;
    private void Start()
    {
        //GameManager�����C����ʎ��ɋL�^���Ă����V�[������PlayerPrefs���玝���Ă���
        _oneBeforeSceneName = PlayerPrefs.GetString("Scene");
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEVolume");
        _timeRecordText.text = "Time: " + PlayerPrefs.GetFloat("Time").ToString("F2");
        if (_isClearScene)
        {
            _clearText.text = $"-{_oneBeforeSceneName}-\r\n-COMPLETE-";
            _difficultyText.text = "Difficulty: " + PlayerPrefs.GetString("DIFF").ToUpper();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(_oneBeforeSceneName);
            PlayerPrefs.DeleteKey("Time");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Title");
            PlayerPrefs.DeleteKey("Time");
        }
    }
}
