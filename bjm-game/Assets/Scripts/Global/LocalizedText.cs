// Reference: https://unity3d.com/de/learn/tutorials/topics/scripting/localization-manager

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string key;

    void Start()
    {

        try
        {
            if (GetComponent<TextMeshProUGUI>())
            {
                TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
                text.text = LocalizationManager.instance.GetLocalizedValue(key);
            }
            if (GetComponent<TextMeshPro>())
            {
                TextMeshPro text = GetComponent<TextMeshPro>();
                text.text = LocalizationManager.instance.GetLocalizedValue(key);
            }
        }catch(NullReferenceException e)
        {
            string GOpath;
            if(transform.parent != null)
            {
                GOpath = transform.parent.name + " > " + gameObject.name;
            }
            else
            {
                GOpath = gameObject.name;
            }

            Debug.LogError("NO TEXT on "+ GOpath + ": " + e);

        }

    }
}
