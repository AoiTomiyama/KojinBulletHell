using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// フェードインとフェードアウトを行うスクリプト。
/// </summary>
public class FadeInOut : MonoBehaviour
{
    [SerializeField, Header("開始時にフェードアウトを入れるか")]
    private bool _isStartFadeOut = true;
    /// <summary>アニメーターのコンポーネント</summary>
    private Animator _anim;
    /// <summary>フェードイン終了後にシーン遷移したいため、一時的にシーン名を保存</summary>
    private string _sceneName;
    private void Awake()
    {
        _sceneName = null;
        _anim = GetComponent<Animator>();
        if (_isStartFadeOut) _anim.Play("FadeOut");
    }
    /// <summary>
    /// フェードイン後、シーンを移動する。
    /// </summary>
    /// <param name="sceneName">移動先のシーン名</param>
    public void FadeInAndChangeScene(string sceneName)
    {
        if (_sceneName != null)
        {
            Debug.LogWarning($"Started FadeIn is already exists, Ongoing FadeIn is {_sceneName}");
            return;
        }
        Debug.Log($"FadeIn started, Next scene is {sceneName}");
        _anim.Play("FadeIn");
        _sceneName = sceneName;
    }
    /// <summary>
    /// アニメーションイベント用のメソッド。
    /// </summary>
    private void ChangeScene() => SceneManager.LoadScene(_sceneName);
}
