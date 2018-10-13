using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour {

    public GameObject npc;
    public int spawnTime = 5;
    public Transform spawnPoint;

    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
    
    // Update is called once per frame
    void Update () {
    }

    void Spawn() {
        Instantiate(npc, spawnPoint.position, spawnPoint.rotation);
    }
}
