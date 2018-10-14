using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [Header("Reward Configuration")]
    public int rewardSatisfiedRide = 5;
    public int penaltyInQueue = -1;
    public int penaltyUnsatisfiedRide = -5;

    [Header("Game Phase Configuration")]
    public Color goodColor;
    public Color mediumColor;
    public Color badColor;
    public int mediumTopLimit = 70;
    public int mediumBottomLimit = 30;

    [Header("NPC Configuration")]
    public int spawnFrequency = 5;
    public int spawnAmount = 1;
    public int idleTimeMin = 5;
    public int idleTimeMax = 5;

    [Header("Player Configuration")]
    public float playerSpeed = 7;

    [Header("Level Configuration")]
    public int baseVisitorsNeeded = 20;
    public float multiplierPerStage = 2;
}
