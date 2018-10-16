using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisitorManager : MonoBehaviour
{
    GameController game;

    public int visitorsSatisfied;

    public List<NPC> allVisitors = new List<NPC>();

    [Header("Stages")]
    int currentVisitorsNeeded;

    int nextUpdate = 1;

    HUDManager hud;


    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<GameController>();

        currentVisitorsNeeded = game.baseVisitorsNeeded;

        hud = FindObjectOfType<HUDManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextUpdate)
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
        if(game.gameState != GameController.state.RUNNING)
        {
            return;
        }
        if(allVisitors.Count == 0)
        {
            return;
        }
        int npcCount = allVisitors.Count;
        int globalHappinessPoints = 0;
        for(int i = 0; i < npcCount; i++)
        {
            globalHappinessPoints += allVisitors[i].GetHappiness();
        }

        game.happinessPercentage = globalHappinessPoints / npcCount;
        if(hud == null)
        {
            hud = FindObjectOfType<HUDManager>();
        }

        hud.UpdateHappiness(game.happinessPercentage);
        game.CheckGameOver();
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
        currentVisitorsNeeded = Mathf.RoundToInt(currentVisitorsNeeded * game.multiplierPerStage);
        FindObjectOfType<ParkManager>().CreateNewAttraction();
        game.currentLevel++;
    }
}
