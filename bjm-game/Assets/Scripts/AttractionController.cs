﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: NPC status queue


public class AttractionController : MonoBehaviour {

    // Duration of one run
    public double duration = 120;
    public int npcAmount = 10;

    private bool running = false;
    private double timeLeft;

    private int nextUpdate = 1;

    private Ferris_Wheel wheel;
    public List<GameObject> npcsActive;
    private Queue<GameObject> npcsWaiting;

    // Use this for initialization
    void Start() {
        timeLeft = duration;
        wheel = gameObject.GetComponent<Ferris_Wheel>();
    }

    // Update is called once per frame
    void Update() {
        if (Time.time >= nextUpdate) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }

    void OnGUI() {
        Event e = Event.current;
        if (e.isKey) {
            if(e.keyCode == KeyCode.A) {
                Debug.Log("Attraction started");
                StartAttraction();
            } else if(e.keyCode == KeyCode.B) {
                Debug.Log("Attraction stopped");
                StopAttraction();
            }
        }
    }

    // Update is called once per second
    void UpdateEverySecond() {
        if (running) {
            if (timeLeft > 0) {
                --timeLeft;
            } else {
                StopAttraction();
                // ToDo: Send Attraction stopped signal
            }
        }
    }

    void StartAttraction() {
        if (!running) {
            // Move waiting NPCs to attraction
            for (int i = 0; i < npcAmount; ++i) {
                if (npcsWaiting.Count == 0) {
                    break;
                }
                GameObject npc = npcsWaiting.Dequeue();
                NPC npcScript = npc.GetComponent<NPC>();
                npcScript.SetStatus(NPC.status.ATTRACTION);

                npcsActive.Add(npc);
            }

            // Only start attraction if there are some NPCS who wanna drive
            if (npcsActive.Count > 0) {
                // Start attraction
                timeLeft = duration;
                running = true;
                wheel.Activate();
            } else {
                // TODO: Throw error message - No NPCS
            }
        }
    }

    void StopAttraction() {
        if (running) {
            running = false;
            wheel.Deactivate();

            int happinessReward = 0;

            // Abort or regular stop?
            if (timeLeft > 0) {
                happinessReward = -5;
            } else {
                happinessReward = 5;
            }

            while (npcsActive.Count > 0) {
                GameObject npc = npcsActive[0];
                npcsActive.Remove(npc);
                NPC npcScript = npc.GetComponent<NPC>();

                npcScript.SetStatus(NPC.status.WALKING);
                npcScript.AddHappiness(happinessReward);
            }
        }
    }
}