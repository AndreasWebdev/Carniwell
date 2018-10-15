using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PlayerPrefsConstants
{

    public static string HighscoreTimeKey = "HighscoreTime";


    public static void SetHighscoreTime(float _score)
    {
        PlayerPrefs.SetFloat(HighscoreTimeKey, _score);
    }

    public static float GetHighscoreTime()
    {
        return PlayerPrefs.GetFloat(HighscoreTimeKey);
    }

    public static string GetHighscoreTimeString()
    {
        float time = GetHighscoreTime();
        //float milliseconds = (currentTime * 100) % 100;
        float seconds = time % 60;
        float minutes = time / 60;
        //int hours;
        string timeString = Mathf.Floor(minutes).ToString("00") + ":" + Mathf.Floor(seconds).ToString("00");

        return timeString;
    }
}
