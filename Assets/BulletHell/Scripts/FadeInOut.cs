using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// フェードインとフェードアウトを行うスクリプト。
/// 現状フェードアウトのみ
/// </summary>
public class FadeInOut : MonoBehaviour
{
    [Header("フェードインにかかる時間")]
    [SerializeField]
    float _fadeInTimer = 1f;

    Tween _tween;
    /// <summary>Imageコンポーネントを取得</summary>
    Image _image;
    void Start()
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
