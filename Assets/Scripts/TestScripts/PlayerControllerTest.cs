using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerControllerTest : MonoBehaviour
{
    public LayerMask layer;
    RaycastHit hit;

    private NavMeshAgent playerAgent;
    private Rigidbody playerRB;

    public Vector3 playerPos;
    public Vector3 mousePos;
    public Vector3 minusPos;

    bool hasJumped = false;

    public float jumpTimer = 0.2f;
    public bool isControlling = true;

    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        playerRB = GetComponent<Rigidbody>();
    }
    void Update()
    {
    Move();
    Jump();
    }
    void Move() 
    {
        float downSpeed = 5;
        Vector3 moveDown = new Vector3 (0, -1, 0);
        playerPos = playerAgent.transform.position;

        Ray cameraRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1) && isControlling == true)
        {

            if (Physics.Raycast(cameraRayCast, out hit, 10000, layer))
            {
                playerAgent.SetDestination(hit.point);
                mousePos = hit.point;
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && isControlling == true)
        {
            playerAgent.SetDestination(playerPos);
        }
        else if (Input.GetKeyDown(KeyCode.S) && isControlling == false) 
        {
            playerRB.velocity = (moveDown * downSpeed);
        }
    }
    void Interact() 
    { 
    
    }

    void Jump()
    {
        int jumpForce = 3;
        Vector3 jumpDir = new Vector3(0, 3, 1);

        if (Input.GetKeyDown(KeyCode.W) && hasJumped == false)
        {
            playerAgent.enabled = false;
            playerRB.velocity = transform.TransformDirection(jumpDir) * jumpForce;
            hasJumped = true;
            isControlling = false;

        }
        Vector3 stopSpin = new Vector3(0, 0, 0);
        Vector3 rayOffset = new Vector3(0, -1, 0);
        Ray downRay = new Ray(transform.position + rayOffset, transform.up * -1);
        RaycastHit platformHit;

        if (Physics.Raycast(downRay, out platformHit, 0.3f, layer))
        {
            Debug.DrawLine(downRay.origin, downRay.origin + downRay.direction * 1, Color.green);

            if (jumpTimer <= 0)
            {
                playerAgent.enabled = true;
                jumpTimer = 0.2f;
                hasJumped = false;
                playerRB.angularVelocity = stopSpin;
                playerRB.velocity = stopSpin;
                isControlling = true;
            }
        }
        else
        {
            Debug.DrawLine(downRay.origin, downRay.origin + downRay.direction * 1, Color.red);

        }

        if (hasJumped == true)
        {
            jumpTimer = jumpTimer - Time.deltaTime;
        }

    }
}