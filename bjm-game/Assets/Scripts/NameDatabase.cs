using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NameDatabase : MonoBehaviour
{

    public NPCNames npcNames;

    public static  NameDatabase instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start ()
    {
        npcNames = LoadNamesFromDocument();
    }

    NPCNames LoadNamesFromDocument()
    {
        string jsonNames = Resources.Load<TextAsset>("npc_names").text;
            
        return JsonUtility.FromJson<NPCNames>(jsonNames);
    }

    public string GetRandomName()
    {
        return npcNames.names[Random.Range(0,npcNames.names.Length)];
    }

}

[System.Serializable]
public class NPCNames
{
    public string[] names;
}
