using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// チュートリアル画面で、特定の範囲に入ったらテキストを表示させるスクリプト
/// </summary>
public class ShowTutorialGuide : MonoBehaviour, IPausable
{
    [SerializeField, Header("表示させるテキスト")]
    private List<TextMeshProUGUI> _texts;
    /// <summary>Tween完了前にシーン移動した際にKillできるように保存</summary>
    private List<Tween> _tweens = new();
    /// <summary>テキストの表示状態</summary>
    private bool _isVisible;
    private void Start()
    {
        _texts.ForEach(text => text.color = new(text.color.r, text.color.g, text.color.b, 0));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isVisible)
        {
            _isVisible = true;
            foreach (var text in _texts)
            {
                _tweens.Add(text.transform.DOMoveY(text.transform.position.y - 30f, 0.7f));
                _tweens.Add(text.DOFade(1, 0.8f));
            }
        }
    }
    private void OnDisable()
    {
        _tweens.ForEach(tw => tw.Kill());
    }
    public void Pause()
    {
        _tweens.ForEach(tw => tw.Pause());
        _texts.ForEach(text => text.enabled = false);
    }

    public void Resume()
    {
        _tweens.ForEach(tw => tw.Play());
        _texts.ForEach(text => text.enabled = true);
    }
}
