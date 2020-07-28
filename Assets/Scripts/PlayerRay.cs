using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerRay : MonoBehaviour
{
    public GameObject hitBox;
    public float hitTime = 2f;
    public bool hasHit = false;
    public bool hasStarted = false;
    public LayerMask layer;
    RaycastHit hit;

    private NavMeshAgent playerAgent;
    private Rigidbody playerRB;

    public Vector3 playerPos;
    public Vector3 mousePos;
    public Vector3 minusPos;

    bool hasJumped = false;

    public float jumpTimer = 0.2f;
    public int speed = 500;

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
    void FixedUpdate() 
    {
        Attack();
    }

    void Jump() 
    {
        int jumpForce = 3;
        Vector3 jumpDir = new Vector3(0, 3, 1);

        if (Input.GetKeyDown(KeyCode.W) && hasJumped == false) 
        {
            playerAgent.enabled = false;
            playerRB.velocity =  transform.TransformDirection(jumpDir) * jumpForce;
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
                Debug.DrawLine(cameraRayCast.origin, cameraRayCast.origin + cameraRayCast.direction * 100, Color.red);
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
    void Attack() 
    {
        Ray cameraRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && isControlling == true)
        {
            if (Physics.Raycast(cameraRayCast, out hit, 10000, layer)   )
            {
                mousePos = hit.point;
                minusPos = mousePos - playerPos;
                hasStarted = true;
                playerAgent.destination = playerPos;
            }
        }
        if (hasHit == true)
        {
            hitTime = hitTime - 0.1f;

            if (hitTime <= 0)
            {
                hitBox.SetActive(false);
                hitTime = 2f;
                hasHit = false;
            }
        }

        if (hasStarted == true)
        {
            Vector3 direction = mousePos - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);

            
            Vector3 dir = (mousePos - transform.position).normalized;
            float dot = Vector3.Dot(dir, transform.forward);
            if (dot >= 0.70f) 
            {
                hitBox.SetActive(true);
            }
            
            if (dot >= 0.95f) 
            {
                //Debug.Log("FUCK");
                hasStarted = false;
                hasHit = true;
            }
        }
    }

    void OnCollisionEnter(Collision col) 
    {

        if (col.gameObject.CompareTag("Enemy")) 
        {
            //playerAgent.SetDestination(playerPos);
        }
        if (col.gameObject.CompareTag("Ground")) 
        {
       
        }
    }
}