using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ferris_Wheel : MonoBehaviour
{
    public bool active = false;
    public float speed = 10; // Drehgeschwindigkeit
    public Transform RotatingObject; //Das Hauptrad
    public Transform[] cabins;


    public void Update()
    {
        if (!active) return;
        RotatingObject.Rotate(Vector3.right * (speed * Time.deltaTime));

        foreach(Transform cabin in cabins)
        {
            cabin.Rotate(-Vector3.right * (speed * Time.deltaTime));
        }
    }

    //DIESE METHODE ZUM STARTEN DES DREHVORGANGS NUTZEN
    public void Activate()
    {
        active = true;
    }

    //DIESE METHODE ZUM STOPPEN DES DREHVORGANGS NUTZEN
    public void Deactivate()
    {
        active = false;
    }
}
