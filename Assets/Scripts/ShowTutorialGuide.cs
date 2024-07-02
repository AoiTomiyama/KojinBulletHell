using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// �`���[�g���A����ʂŁA����͈̔͂ɓ�������e�L�X�g��\��������X�N���v�g
/// </summary>
public class ShowTutorialGuide : MonoBehaviour
{
    [Header("�\��������e�L�X�g")]
    [SerializeField]
    private TextMeshProUGUI _text;
    /// <summary>Tween�����O�ɃV�[���ړ������ۂ�Kill�ł���悤�ɕۑ�</summary>
    private List<Tween> _tweens = new();
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
