using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnPointerEnterButton : MonoBehaviour, IPointerEnterHandler
{
    public UnityEvent OnMouseEnter;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter.Invoke();
    }
}
