using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ParkStatistics : MonoBehaviour {

    [Header("Visitor Count")]
    public TextMeshPro statsText;

    int nextUpdate = 1;

    VisitorManager visitorManager;
    ParkManager parkManager;
    void Start () {
        visitorManager = FindObjectOfType<VisitorManager>();
        parkManager = FindObjectOfType<ParkManager>();
    }

    void Update () {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }

    void UpdateEverySecond()
    {
        string statisticsText = "";

        statisticsText += visitorManager.allVisitors.Count.ToString() + " Besucher";


        int completedRides = 0;
        for (int i = 0; i < parkManager.activeAttractions.Count; i++)
        {
            completedRides += parkManager.activeAttractions[i].completedRides;
        }
        statisticsText += "\n" + completedRides + " gefahrene Attraktionen";

        statsText.text = statisticsText;

    }
}
