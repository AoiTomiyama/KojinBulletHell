using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��ʑS�̂Ƀt���b�V����������X�N���v�g
/// </summary>
public class FlashEffect : MonoBehaviour
{
    private Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();
    }
    /// <summary>
    /// ��ʑS�̂����点��B
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
