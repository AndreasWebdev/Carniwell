using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour {

    GameController game;

    public GameObject npc;
    public Transform spawnPoint;

    // Use this for initialization
    void Start () {
        game = FindObjectOfType<GameController>();

        InvokeRepeating("Spawn", game.spawnFrequency, game.spawnFrequency);
    }
    
    // Update is called once per frame
    void Update () {
    }

    void Spawn() {
        for (int i = 0; i < game.spawnAmount; ++i) {
            Instantiate(npc, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
