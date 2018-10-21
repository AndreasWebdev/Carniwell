using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
    public bool eventReceived = false;

    CanvasGroup canvasGroup;

     void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void freeze()
    {
        Time.timeScale = 0;
        eventReceived = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
