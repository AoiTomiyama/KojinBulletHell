using UnityEngine;
using UnityEngine.EventSystems;

public class ShowStageInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField, Header("InformationShower����R���|�[�l���g�������Ă���")]
    private Animator _anim;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _anim.Play("ShowInfo");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _anim.Play("HideInfo");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _anim.Play("HideInfo");
    }
}
