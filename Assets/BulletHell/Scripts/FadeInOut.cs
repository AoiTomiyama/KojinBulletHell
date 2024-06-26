using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [Header("�t�F�[�h�C���ɂ����鎞��")]
    [SerializeField]
    float _fadeInTimer = 1f;
    /// <summary>Image�R���|�[�l���g���擾</summary>
    Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        _image.enabled = true;
        var color = _image.color;
        var elapsedTime = 0f;
        while (elapsedTime < _fadeInTimer)
        {
            var elapsedRate = Mathf.Min(elapsedTime / _fadeInTimer, 1f);
            color.a = 1.0f - elapsedRate;
            yield return null;
            elapsedTime += Time.deltaTime;
            _image.color = color;
        }
        _image.enabled = false;
    }
}
