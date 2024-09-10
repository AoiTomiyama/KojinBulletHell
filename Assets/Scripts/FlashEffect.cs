using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 画面全体にフラッシュをかけるスクリプト
/// </summary>
public class FlashEffect : MonoBehaviour, IPausable
{
    private Image _image;
    private Tween _tween;
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
        _tween = _image.DOFade(0, duration)
            .SetEase(Ease.OutExpo)
            .OnComplete(() => _image.enabled = false);
    }

    public void Pause()
    {
        _tween?.Pause();
    }

    public void Resume()
    {
        _tween?.Play();
    }
}
