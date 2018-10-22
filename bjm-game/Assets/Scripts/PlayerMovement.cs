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
    bool movementAlertShowed = false;

    void Start()
    {
        game = FindObjectOfType<GameController>();
        joystick = FindObjectOfType<Joystick>();
        joystick.gameObject.SetActive(false);
    }

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        hud = FindObjectOfType<HUDManager>();
    }

    public void OnGameStarted()
    {
        joystick.gameObject.SetActive(true);
    }

    public void OnGameStopped()
    {
        joystick.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(game.gameState == GameController.state.RUNNING && !isLocked)
        {
            Move();
        }
        else if(isLocked)
        {
            if(joystick.Horizontal != 0 && joystick.Vertical != 0 && !movementAlertShowed)
            {
                // Show this alert only once per lock cycle
                movementAlertShowed = true;
                hud.ShowAlert(LocalizationManager.instance != null ? LocalizationManager.instance.GetLocalizedValue("alert_wait_until_attraction_over") : "Warte bis die Attraktion beendet ist.");
            }
            else if(joystick.Horizontal == 0 && joystick.Vertical == 0 && movementAlertShowed)
            {
                movementAlertShowed = false;
            }
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
        movementAlertShowed = false;
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
