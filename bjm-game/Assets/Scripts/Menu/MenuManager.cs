using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class MenuManager : MonoBehaviour
{
    [Header("Credits")]
    public DirectorControlPlayable timelineCredits;

    public void buttonStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void buttonSettings()
    {

    }

    public void buttonCredits()
    {
        timelineCredits.director.Play();
    }
}
