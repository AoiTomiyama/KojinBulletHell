using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// �`���[�g���A����ʂŁA����͈̔͂ɓ�������e�L�X�g��\��������X�N���v�g
/// </summary>
public class ShowTutorialGuide : MonoBehaviour, IPausable
{
    [SerializeField, Header("�\��������e�L�X�g")]
    private TextMeshProUGUI _text;
    /// <summary>Tween�����O�ɃV�[���ړ������ۂ�Kill�ł���悤�ɕۑ�</summary>
    private List<Tween> _tweens = new();
    /// <summary>�e�L�X�g�̕\�����</summary>
    private bool _isVisible;
    private void Start()
    {
        _text.color = new(_text.color.r, _text.color.g, _text.color.b, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isVisible)
        {
            _isVisible = true;
            _tweens.Add(_text.transform.DOMoveY(_text.transform.position.y - 30f, 0.7f));
            _tweens.Add(_text.DOFade(1, 0.8f));
        }
    }
    private void OnDisable()
    {
        _tweens.ForEach(tw => tw.Kill());
    }
    public void Pause()
    {
        _tweens.ForEach(tw => tw.Pause());
        _text.enabled = false;
    }

    public void Resume()
    {
        _tweens.ForEach(tw => tw.Play());
        _text.enabled = true;
    }
}
