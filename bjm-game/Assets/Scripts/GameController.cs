﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public enum state {
        STOPPED,
        RUNNING,
        PAUSED,
        GAMEOVER
    }

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

    [Header("Game Information")]
    public state gameState = state.STOPPED;

    public void PauseGame() {
        if (gameState == state.RUNNING) {
            Time.timeScale = 0;

            Scene active = SceneManager.GetActiveScene();
            GameObject[] roots = active.GetRootGameObjects();
            foreach (GameObject root in roots) {
                root.BroadcastMessage("onGamePaused", SendMessageOptions.DontRequireReceiver);
            }

            gameState = state.PAUSED;
        } else if (gameState == state.PAUSED) {
            Time.timeScale = 1;

            Scene active = SceneManager.GetActiveScene();
            GameObject[] roots = active.GetRootGameObjects();
            foreach (GameObject root in roots) {
                root.BroadcastMessage("onGameResumed", SendMessageOptions.DontRequireReceiver);
            }

            gameState = state.RUNNING;
        } else {
            Debug.Log("Wrong game state detected");
        }
    }
}