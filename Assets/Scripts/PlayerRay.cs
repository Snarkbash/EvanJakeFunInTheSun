using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerRay : MonoBehaviour
{
    public GameObject hitBox;
    public float hitTime = 2f;
    public bool hasHit = false;

    public LayerMask layer;
    RaycastHit hit;

    private NavMeshAgent playerAgent;
    private Rigidbody playerRB;

    public Vector3 playerPos;
    public Vector3 mousePos;
    public Vector3 minusPos;

    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {

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
        playerPos = playerAgent.transform.position;
        
        Ray cameraRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
           
            if (Physics.Raycast(cameraRayCast, out hit, 10000, layer))
            {
                Debug.DrawLine(cameraRayCast.origin, cameraRayCast.origin + cameraRayCast.direction * 100, Color.red);
                playerAgent.SetDestination(hit.point);
                mousePos = hit.point;

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            if (Physics.Raycast(cameraRayCast, out hit, 10000, layer) && hasHit == false)
            {
                mousePos = hit.point;
                minusPos = mousePos - playerPos;

                playerAgent.transform.LookAt(mousePos);
                hitBox.SetActive(true);
                hasHit = true;
                playerAgent.destination = playerPos;


            }
        }
    }

    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.CompareTag("Enemy")) 
        {
            playerRB.velocity = new Vector3(0, 0, 0);
            Debug.Log("col");
        }
    }
}