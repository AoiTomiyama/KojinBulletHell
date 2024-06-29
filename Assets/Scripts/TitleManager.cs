using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ボタンから読み取り、OnStartButtonClicked関数を実行してシーンを移動させる。
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("ボタンが押されたときのSE")]
    [SerializeField]
    AudioClip _pressedSE;
    public void OnStartButtonClicked(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
