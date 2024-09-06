using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 画面全体にフラッシュをかけるスクリプト
/// </summary>
public class FlashEffect : MonoBehaviour
{
    private Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();
    }
    /// <summary>
    /// 画面全体を光らせる。
    /// </summary>
    public void Flash(float duration = 0.2f)
    {
        _image.enabled = true;
        var c = _image.color;
        c.a = 1;
        _image.color = c;
        _image.DOFade(0, duration).
        SetEase(Ease.OutExpo).
        OnComplete(() => _image.enabled = false);
    }
}
