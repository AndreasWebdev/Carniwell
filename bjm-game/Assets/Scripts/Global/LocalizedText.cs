// Reference: https://unity3d.com/de/learn/tutorials/topics/scripting/localization-manager

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
#if !UNITY_EDITOR
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.instance.GetLocalizedValue(key);
#endif
    }
}
