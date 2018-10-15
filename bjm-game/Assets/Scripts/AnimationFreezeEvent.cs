using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationFreezeEvent : MonoBehaviour
{
    public bool eventReceived = false;


    public void freeze()
    {
        if (!eventReceived)
        {
            Time.timeScale = 0;
            eventReceived = true;
        }
    }
}
