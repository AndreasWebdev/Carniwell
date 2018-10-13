using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorManager : MonoBehaviour {
    public int visitorsSatisfied;
    public float happinessPercentage;

    public List<NPC> allVisitors = new List<NPC>();


    [Header("Stages")]
    public int baseVisitorsNeeded = 20;
    public float multiplierPerStage = 2;
    public int currentVisitorsNeeded;

    private int nextUpdate = 1;
    // Use this for initialization
    void Start () {
        currentVisitorsNeeded = baseVisitorsNeeded;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }

    void UpdateEverySecond()
    {
        CalculateHappiness();
    }

    public void CalculateHappiness()
    {
        if(allVisitors.Count == 0){
            return;
        }
        int npcCount = allVisitors.Count;
        int globalHappinessPoints = 0;
       for(int i = 0; i < npcCount; i++)
       {
            globalHappinessPoints += allVisitors[i].GetHappiness();
       }

        happinessPercentage = globalHappinessPoints / npcCount;
    }

    public void AddSatisfiedVisitors(int _amount)
    {
        visitorsSatisfied += _amount;
        if(visitorsSatisfied >= currentVisitorsNeeded)
        {
            NextStage();
            
        }
    }
    public void NextStage()
    {
        currentVisitorsNeeded = Mathf.RoundToInt(currentVisitorsNeeded * multiplierPerStage);
        FindObjectOfType<ParkManager>().CreateNewAttraction();
    }


}
