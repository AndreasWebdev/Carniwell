using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour {

    public Image cooldownImage;
    public float cooldown = 0;

    public bool cooldownRunning;

    private void Update() {
        if (cooldownRunning) {
            cooldownImage.fillAmount += 1 / cooldown * Time.deltaTime;
        }
        else {
            cooldownImage.fillAmount = 0;
        }

        if(cooldownImage.fillAmount >= 1) {
            cooldownRunning = false;
        }
    }

}
