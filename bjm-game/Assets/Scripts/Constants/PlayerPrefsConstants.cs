using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PlayerPrefsConstants
{
    //Score
    public static string HighscoreTimeKey = "HighscoreTime";

    public static string HighscorePlayerNameKey = "HighscorePlayerName";

    //Settings
    public static string SFXVolumeKey = "SFX_Volume";
    public static string MusicVolumeKey = "Music_Volume";
    public static string AnnouncerVolumeKey = "Announcer_Volume";


    #region Highscore
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

    public static string GetHighscorePlayerName()
    {
        if (PlayerPrefs.HasKey(HighscorePlayerNameKey))
        {
            return PlayerPrefs.GetString(HighscorePlayerNameKey);
        }else
        {
            return string.Empty;
        }
    }
    public static void SetHighscorePlayerName(string _name)
    {
        PlayerPrefs.SetString(HighscorePlayerNameKey,_name);
    }
    #endregion

    #region Settings

    public static void SetSFXVolume(float _vol)
    {

        _vol = Mathf.Clamp(_vol, 0, 1);
        PlayerPrefs.SetFloat(SFXVolumeKey, _vol);
    }

    public static void SetMusicVolume(float _vol)
    {
        _vol = Mathf.Clamp(_vol, 0, 1);
        PlayerPrefs.SetFloat(MusicVolumeKey, _vol);
    }

    public static void SetAnnouncerVolume(float _vol)
    {
        _vol = Mathf.Clamp(_vol, 0, 1);
        PlayerPrefs.SetFloat(AnnouncerVolumeKey, _vol);
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFXVolumeKey,1);
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey,1);
    }

    public static float GetAnnouncerVolume()
    {
        return PlayerPrefs.GetFloat(AnnouncerVolumeKey,1);
    }

    #endregion
}
