using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    GameController game;

    Rigidbody playerRigidbody;

    public Animator anim;

    protected Joystick joystick;

    HUDManager hud;

    bool isLocked = false;

    void Start()
    {
        game = FindObjectOfType<GameController>();
        joystick = FindObjectOfType<Joystick>();
    }

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        hud = FindObjectOfType<HUDManager>();
    }

    void FixedUpdate()
    {
        if(game.gameState == GameController.state.RUNNING && !isLocked)
        {
            Move();
        }
    }

    void Move()
    {
        if(joystick.Horizontal != 0 && joystick.Vertical != 0)
        {
            anim.SetBool("moving", true);

            Vector3 moveDir = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

            Quaternion rotation = Quaternion.LookRotation(moveDir);
            rotation.x = rotation.z = 0;
            playerRigidbody.MoveRotation(rotation);

            moveDir.y = 0.2f;
            moveDir = moveDir.normalized * game.playerSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + moveDir);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    public void UnlockMovement()
    {
        isLocked = false;
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = true;
    }

    public void LockMovement()
    {
        isLocked = true;
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<AttractionController>() != null)
        {
            hud.SetupCurrentAttraction(other.GetComponentInParent<AttractionController>());
        }
        else if(other.GetComponentInParent<TreeController>() != null)
        {
            TreeController treeController = other.GetComponentInParent<TreeController>();
            treeController.UpdateTransparency(0.2f);
        }
        else
        {
            // Nothing to do
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponentInParent<AttractionController>())
        {
            hud.LeaveAttraction();
        }
        else if(other.GetComponentInParent<TreeController>() != null)
        {
            TreeController treeController = other.GetComponentInParent<TreeController>();
            treeController.UpdateTransparency(1f);
        }
        else
        {
            // Nothing to do
        }
    }
}
