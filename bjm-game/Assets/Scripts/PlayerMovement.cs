﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 10f;

    Rigidbody playerRigidbody;
    float touchRayLength = 100f;
    int floorMask;

    public Animator anim;

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
        if (Input.GetMouseButton(0))
        {
            Ray touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit floorHit;
            if (Physics.Raycast(touchRay, out floorHit, touchRayLength, floorMask))
            {
                Vector3 moveDir = floorHit.point - transform.position;
                moveDir.y = 0.2f;

                moveDir = moveDir.normalized * speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + moveDir);

                Quaternion rotation = Quaternion.LookRotation(moveDir);
                playerRigidbody.MoveRotation(rotation);

                anim.SetBool("moving",true);
                
            }
        }else
        {
            anim.SetBool("moving", false);
        }
        
    }
}
