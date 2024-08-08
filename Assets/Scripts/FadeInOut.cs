using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �t�F�[�h�C���ƃt�F�[�h�A�E�g���s���X�N���v�g�B
/// ����t�F�[�h�A�E�g�̂�
/// </summary>
public class FadeInOut : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�C���ɂ����鎞��")]
    private float _fadeInTimer = 1f;

    /// <summary>Tween�����O�ɃV�[���ړ������ۂ�Kill�ł���悤�ɕۑ�</summary>
    private Tween _tween;
    /// <summary>Image�R���|�[�l���g���擾</summary>
    private Image _image;
    void Start()
    {
        FadeOut();
    }
    void FadeOut()
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
