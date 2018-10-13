using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public enum status {
        QUEUE,
        WALKING,
        ATTRACTION
    }

    private int happiness;
    private status currentStatus;

    private int nextUpdate = 1;

    // Use this for initialization
    void Start() {
        happiness = 100;
        currentStatus = status.WALKING;
    }

    // Update is called once per frame
    void Update() {
        // If the next update is reached
        if (Time.time >= nextUpdate) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }

    // Update is called once per second
    void UpdateEverySecond() {
        if (currentStatus.Equals(status.QUEUE)) {
            if (happiness > 0) {
                AddHappiness(-1);
            }
        //} else if (currentStatus.Equals(status.ATTRACTION)) {
        //    if (happiness < 100) {
        //        ++happiness;
        //    }
        }
    }

    public status GetStatus() {
        return currentStatus;
    }

    public void SetStatus(status newStatus) {
        currentStatus = newStatus;
    }

    public int GetHappiness() {
        return happiness;
    }

    public void AddHappiness(int reward) {
        happiness += reward;

        if(happiness <= 0) {
            // Remove NPC
        }
    }
}
