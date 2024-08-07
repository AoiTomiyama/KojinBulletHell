using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 画面全体にフラッシュをかけるスクリプト
/// </summary>
public class FlashEffect : MonoBehaviour
{
    public static FlashEffect Instance { get; private set; }
    private Image _image;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _image = GetComponent<Image>();
    }
    /// <summary>
    /// 画面全体を光らせる。
    /// 毎回StartCoroutine書くの面倒だから対策した。
    /// </summary>
    public void Flash()
    {
        StartCoroutine(FlashEumerator());
    }
    private IEnumerator FlashEumerator()
    {
        _image.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _image.enabled = false;
    }
}
