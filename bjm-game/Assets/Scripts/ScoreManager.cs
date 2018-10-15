using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ScoreManager : MonoBehaviour
{
    bool counting;
    public float currentTime;

    HUDManager hud;

    string timeString = "00:00";
    // Use this for initialization
    void Start()
    {
        hud = FindObjectOfType<HUDManager>();
        counting = true;
    }

    void Update()
    {
        if(!counting) return;
        currentTime += Time.deltaTime;

        //float milliseconds = (currentTime * 100) % 100;
        float seconds = currentTime % 60;
        float minutes = currentTime / 60;
        //int hours;
       timeString = Mathf.Floor(minutes).ToString("00") + ":" + Mathf.Floor(seconds).ToString("00");

        hud.scoreText.text = timeString;
    }

    public void GameOver()
    {
        counting = false;

        CheckForNewHighscore();
    }

    void CheckForNewHighscore()
    {
        if(currentTime > PlayerPrefsConstants.GetHighscoreTime())
        {
            PlayerPrefsConstants.SetHighscoreTime(currentTime);
        }
    }

    public string GetScoreString()
    {
        return timeString;
    }

    public string GetHighscoreString()
    {
        return PlayerPrefsConstants.GetHighscoreTimeString();
    }
}
