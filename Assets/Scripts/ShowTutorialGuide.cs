using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTutorialGuide : MonoBehaviour
{
    [Header("表示させるテキスト")]
    [SerializeField]
    TextMeshProUGUI _text;
    /// <summary>Tween完了前にシーン移動した際にKillできるように保存</summary>
    List<Tween> _tweens = new();
    private void Start()
    {
        var color = _text.color;
        color.a = 0;
        _text.color = color;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _tweens.Add(_text.transform.DOMoveY(_text.transform.position.y - 30f, 0.7f));
            _tweens.Add(_text.DOFade(1, 0.8f).OnComplete(() => Destroy(this.gameObject)));
        }
    }
    private void OnDisable()
    {
        foreach (var tw in _tweens)
        {
            tw.Kill();
        }
    }
}
