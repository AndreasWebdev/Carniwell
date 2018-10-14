using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ScoreManager : MonoBehaviour {
    public float currentTime;


    HUDManager hud;
	// Use this for initialization
	void Start () {
        hud = FindObjectOfType<HUDManager>();
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

        float milliseconds = (currentTime * 100)%100;
        float seconds = currentTime % 60;
        float minutes = currentTime / 60;
        //int hours;
        string timeString = "";
        if(minutes >= 1) {
         timeString = minutes.ToString("F0") + ":" + seconds.ToString("F0") + ":" + milliseconds.ToString("F0");

        }else
        {
            timeString = seconds.ToString("F0") + ":" + milliseconds.ToString("F0");
        }
        hud.scoreText.text = timeString;
    }
    
}
