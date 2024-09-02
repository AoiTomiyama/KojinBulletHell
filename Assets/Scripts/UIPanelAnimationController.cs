using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelAnimationController : MonoBehaviour
{
    [SerializeField, Header("‚±‚±‚ÉInteractionBlocker‚ð“ü‚ê‚é")]
    private GameObject _interactionBlocker;
    private void OnExitAnimationStart()
    {
        _interactionBlocker.SetActive(true);
    }
    private void OnExitAnimationComplete()
    {
        _interactionBlocker.SetActive(false);
        gameObject.SetActive(false);
    }
}
