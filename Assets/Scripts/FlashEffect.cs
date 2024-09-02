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
    /// ����StartCoroutine�����̖ʓ|������΍􂵂��B
    /// </summary>
    public void Flash() => StartCoroutine(FlashEumerator());
    private IEnumerator FlashEumerator()
    {
        _image.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _image.enabled = false;
    }
}
