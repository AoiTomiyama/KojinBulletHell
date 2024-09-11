using TMPro;
using UnityEngine;

/// <summary>
/// ���g���C��ʂ��Ǘ�����X�N���v�g
/// </summary>
public class ClearSceneManager : MonoBehaviour
{
    [SerializeField, Header("���Ԃ̋L�^���o�͂���e�L�X�g")]
    private TextMeshProUGUI _timeRecordText;

    [SerializeField, Header("�I�񂾓�Փx���o�͂���e�L�X�g")]
    private TextMeshProUGUI _difficultyText;

    [SerializeField, Header("�N���A�\��")]
    private TextMeshProUGUI _clearText;

    /// <summary>���O�̃V�[����������ϐ�</summary>
    private string _oneBeforeSceneName;

    private void Start()
    {
        //GameManager�����C����ʎ��ɋL�^���Ă����V�[������PlayerPrefs���玝���Ă���
        _oneBeforeSceneName = PlayerPrefs.GetString("Scene");
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEVolume");

        var record = PlayerPrefs.GetFloat("Time");
        if (PlayerPrefs.HasKey("DIFF_INT") && _oneBeforeSceneName != "")
        {
            SaveDateManager.Instance.AddData(new SaveDateManager.Record(_oneBeforeSceneName, PlayerPrefs.GetInt("DIFF_INT"), record));
        }

        _timeRecordText.text = "Time: " + record.ToString("F2");

        _clearText.text = $"-{_oneBeforeSceneName}-\r\n-COMPLETE-";
        _difficultyText.text = "Difficulty: " + PlayerPrefs.GetString("DIFF").ToUpper();
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
