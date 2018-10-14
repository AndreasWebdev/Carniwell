using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvent : MonoBehaviour {

    public void freeze() {
        Time.timeScale = 0;
    }
}
