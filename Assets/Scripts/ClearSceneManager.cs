using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
        var recordList = SaveDateManager.LoadData();
        var tempData = new SaveDateManager.Record(_oneBeforeSceneName, PlayerPrefs.GetInt("DIFF_INT"), record);

        recordList.Add(tempData);
        if (recordList.Where(data => data._stage == tempData._stage && data._difficulty == tempData._difficulty).Count() >= 4)
        {
            Debug.LogWarning("���������Ȃ肷���Ȃ��悤�ɗv�f���폜");

            var tempList = recordList.Where(data => data._stage == tempData._stage && data._difficulty == tempData._difficulty).OrderBy(data => data._time).Take(3).ToList();
            var tempList2 = recordList.Where(data => data._stage != tempData._stage || data._difficulty != tempData._difficulty).OrderBy(data => data._time).ToList();
            recordList = tempList.Concat(tempList2).ToList();
        }

        SaveDateManager.SaveData(recordList);

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
