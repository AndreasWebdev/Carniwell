using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NameDatabase : MonoBehaviour
{

    public List<string> npcNames;

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

    List<string> LoadNamesFromDocument()
    {
        string rawNames = Resources.Load<TextAsset>("npc_names").text;

        //Old version with json

        //return JsonUtility.FromJson<NPCNames>(rawNames);

        //New version with names seperated by \n
        string[] seperatedNames = rawNames.Trim().Split('\n');
        List<string> namesList = new List<string>();
        foreach(string n in seperatedNames)
        {
            namesList.Add(n);
        }

        return namesList;
    }

    public string GetRandomName()
    {
        return npcNames[Random.Range(0,npcNames.Count)];
    }

}