using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorManager : MonoBehaviour {
    public int visitorsSatisfied;
    public float happinessPercentage;
    [Header("Stages")]
    public int baseVisitorsNeeded = 20;
    public float multiplierPerStage = 2;
    public int currentVisitorsNeeded;
	// Use this for initialization
	void Start () {
        currentVisitorsNeeded = baseVisitorsNeeded;
	}
	
	// Update is called once per frame
	void Update () {
		
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
