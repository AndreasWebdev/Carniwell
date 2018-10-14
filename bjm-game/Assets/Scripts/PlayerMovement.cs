using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

    public float speed = 10f;

    Rigidbody playerRigidbody;

    public Animator anim;

    private bool isMovable = true;

    protected Joystick joystick;

    HUDManager hud;

    private void Start() {
        joystick = FindObjectOfType<Joystick>();
    }

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
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
        if (joystick.Horizontal != 0 && joystick.Vertical != 0) {
            anim.SetBool("moving", true);

            Vector3 moveDir = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

            Quaternion rotation = Quaternion.LookRotation(moveDir);
            rotation.x = rotation.z = 0;
            playerRigidbody.MoveRotation(rotation);

            moveDir.y = 0.2f;
            moveDir = moveDir.normalized * speed * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + moveDir);
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
