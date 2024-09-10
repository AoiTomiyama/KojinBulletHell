using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowStageInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField, Header("InformationShower����R���|�[�l���g�������Ă���")]
    private Animator _anim;
    [SerializeField, Header("�؂�ւ���摜")]
    private Sprite _sprite;
    private Image _image;
    private void Start()
    {
        _image = _anim.GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _anim.Play("InfoEnter");
        _image.sprite = _sprite;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _anim.Play("InfoExit");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _anim.Play("InfoExit");
    }
}
