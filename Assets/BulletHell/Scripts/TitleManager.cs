using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �{�^������ǂݎ��AOnStartButtonClicked�֐������s���ăV�[�����ړ�������B
/// </summary>
public class TitleManager : MonoBehaviour
{
    public void OnStartButtonClicked(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
