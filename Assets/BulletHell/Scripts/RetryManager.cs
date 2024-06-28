using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���g���C��ʂ��Ǘ�����X�N���v�g
/// </summary>
public class RetryManager : MonoBehaviour
{
    [Header("�L�^���o�͂���e�L�X�g")]
    [SerializeField]
    TextMeshProUGUI _timeRecordTMPro;
    private void Start()
    {
        _timeRecordTMPro.text = "Time: " + GameManager.GameTime.ToString("F2");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("BulletHell");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
