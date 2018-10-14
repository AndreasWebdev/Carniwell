using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [Header("Reward Configuration")]
    public int rewardSatisfiedRide = 5;
    public int penaltyInQueue = -1;
    public int penaltyUnsatisfiedRide = -5;
    public Color goodColor, mediumColor, badColor;

    [Header("NPC Configuration")]
    public int spawnFrequency = 5;
    public int spawnAmount = 1;

    [Header("Player Configuration")]
    public float playerSpeed = 7;
}
