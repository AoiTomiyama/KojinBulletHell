using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// フェードインとフェードアウトを行うスクリプト。
/// 現状フェードアウトのみ
/// </summary>
public class FadeInOut : MonoBehaviour
{
    [SerializeField, Header("フェードインにかかる時間")]
    private float _fadeInTimer = 1f;

    /// <summary>Tween完了前にシーン移動した際にKillできるように保存</summary>
    private Tween _tween;
    /// <summary>Imageコンポーネントを取得</summary>
    private Image _image;
    void Start()
    {
        FadeOut();
    }
    void FadeOut()
    {
        _image = GetComponent<Image>();
        _image.enabled = true;
        var color = _image.color;
        color.a = 1f;
        _image.color = color;
        _tween = _image.DOFade(0, _fadeInTimer);
    }

    private void OnDisable()
    {
        _tween.Kill();
    }
}
