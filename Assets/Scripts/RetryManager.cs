using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���g���C��ʂ��Ǘ�����X�N���v�g
/// </summary>
public class RetryManager : MonoBehaviour
{
    [SerializeField, Header("���Ԃ̋L�^���o�͂���e�L�X�g")]
    private TextMeshProUGUI _timeRecordText;

    [SerializeField, Header("�I�񂾓�Փx���o�͂���e�L�X�g")]
    private TextMeshProUGUI _difficultyText;

    [SerializeField, Header("�N���A�\��")]
    private TextMeshProUGUI _clearText;

    [SerializeField, Header("��2���g�p���邩")]
    private bool _isClearScene;

    /// <summary>���O�̃V�[����������ϐ�</summary>
    private string _oneBeforeSceneName;
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
            FindObjectOfType<FadeInOut>().FadeInAndChangeScene(_oneBeforeSceneName);
            PlayerPrefs.DeleteKey("Time");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            FindObjectOfType<FadeInOut>().FadeInAndChangeScene("Title");
            PlayerPrefs.DeleteKey("Time");
        }
    }
}
