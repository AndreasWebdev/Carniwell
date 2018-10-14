using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Credits")]
    public Camera creditsCamera;
    public Camera mainCamera;

    public void buttonStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void buttonSettings()
    {

    }

    public void buttonCredits()
    {
        mainCamera.gameObject.SetActive(false);
        creditsCamera.gameObject.SetActive(true);
    }
}
