﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string path = Application.persistentDataPath + System.DateTime.Now.ToFileTime() + ".jpg";
            Debug.Log(path);
            ScreenCapture.CaptureScreenshot(path,1);
        }
    }
}