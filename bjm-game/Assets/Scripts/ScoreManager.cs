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
        string timeString = Mathf.Floor(minutes).ToString("00") + ":" + Mathf.Floor(seconds).ToString("00");

        hud.scoreText.text = timeString;
    }
    
}
