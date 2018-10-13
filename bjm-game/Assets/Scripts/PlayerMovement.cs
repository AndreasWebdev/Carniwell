using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

    public float speed = 10f;

    Rigidbody playerRigidbody;
    float touchRayLength = 100f;
    int floorMask;

    public Animator anim;

    private bool isMovable = true;

    HUDManager hud;
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
        hud = FindObjectOfType<HUDManager>();
    }

    void FixedUpdate()
    {
        if (isMovable) {
            Move();
        }
    }

    void Move()
    {
        if (Input.GetMouseButton(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                Ray touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit floorHit;
                if (Physics.Raycast(touchRay, out floorHit, touchRayLength, floorMask)) {
                    Vector3 moveDir = floorHit.point - transform.position;

                    Quaternion rotation = Quaternion.LookRotation(moveDir);
                    rotation.x = rotation.z = 0;
                    playerRigidbody.MoveRotation(rotation);

                    moveDir.y = 0.2f;
                    moveDir = moveDir.normalized * speed * Time.deltaTime;
                    playerRigidbody.MovePosition(transform.position + moveDir);



                    anim.SetBool("moving", true);
                }
            } else {
                anim.SetBool("moving", false);
            }
        } else {
            anim.SetBool("moving", false);
        }
    }

    public void unlockMovement() {
        isMovable = true;
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = true;
    }

    public void lockMovement() {
        isMovable = false;
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<AttractionController>() != null)
        {
            hud.SetupCurrentAttraction(other.GetComponentInParent<AttractionController>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<AttractionController>())
        {
            hud.LeaveAttraction();
        }
    }
}
