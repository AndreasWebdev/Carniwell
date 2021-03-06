﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCSpawner : MonoBehaviour
{
    GameController game;

    public GameObject npc;
    public Transform spawnPoint;

    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<GameController>();

        Spawn();
    }

    void Spawn()
    {
        if(game.gameState == GameController.state.RUNNING)
        {
            for(int i = 0; i < game.spawnAmount; ++i)
            {
                Instantiate(npc, spawnPoint.position, spawnPoint.rotation);
            }
        }
        Invoke("Spawn", 1 + game.currentLevel * 2);
    }
}
