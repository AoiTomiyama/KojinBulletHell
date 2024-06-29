using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �t�F�[�h�C���ƃt�F�[�h�A�E�g���s���X�N���v�g�B
/// ����t�F�[�h�A�E�g�̂�
/// </summary>
public class FadeInOut : MonoBehaviour
{
    [Header("�t�F�[�h�C���ɂ����鎞��")]
    [SerializeField]
    float _fadeInTimer = 1f;

    Tween _tween;
    /// <summary>Image�R���|�[�l���g���擾</summary>
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
