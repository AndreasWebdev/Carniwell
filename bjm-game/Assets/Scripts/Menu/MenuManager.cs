using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MenuManager : MonoBehaviour
{
    public PlayableDirector timelineCredits;

    public void buttonStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void buttonSettings()
    {

    }

    public void buttonCredits()
    {
        timelineCredits.Play();
    }
}
