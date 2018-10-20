using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour {
    [Range(0,24)]
    public float turnOnTime = 18, turnOffTime = 8;

    public float turnOn, turnOff;
    DayNightController dnController;
    Light lamp;

    void Start () {
        dnController = FindObjectOfType<DayNightController>();
        lamp = GetComponentInChildren<Light>();
        turnOn = turnOnTime / 24;
        turnOff = turnOffTime / 24;
    }

    // Update is called once per frame
    void Update () {
        ControlLight();
    }


    void ControlLight()
    {

        if(lamp != null)
        {
           
           if(dnController.currentTimeOfDay > turnOff && dnController.currentTimeOfDay < turnOn && lamp.intensity == 1)
            {

                lamp.intensity = 0;
            }
            else if((dnController.currentTimeOfDay < turnOff || dnController.currentTimeOfDay > turnOn) && lamp.intensity == 0)
            {
                Debug.Log("ON");
                lamp.intensity = 1;
            }
        }
    }
}
