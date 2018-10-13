﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 10f;

    Rigidbody playerRigidbody;
    float touchRayLength = 100f;
    int floorMask;


    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
    }

    void FixedUpdate()
    {
            Move();
    }

    void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

            RaycastHit floorHit;
            if (Physics.Raycast(touchRay, out floorHit, touchRayLength, floorMask))
            {
                Vector3 moveDir = floorHit.point - transform.position;
                moveDir.y = 0.2f;

                moveDir = moveDir.normalized * speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + moveDir);

                Quaternion rotation = Quaternion.LookRotation(moveDir);
                playerRigidbody.MoveRotation(rotation);
            }
        }
    }
}