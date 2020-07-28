using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float speed = 1;
    public bool hasJumped = false;
    public GameObject player;
    public LayerMask layer;
    int jumpForce = 5;

    Rigidbody enemyRB;
    NavMeshAgent enemyNav;

    public float jumpTime = 0.2f;
    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        enemyNav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
      
        
        Vector3 rayOffset = new Vector3(0, -1, 0);
        Ray downRay = new Ray(transform.position + rayOffset, transform.up * -1);
        RaycastHit platformHit;

        if (hasJumped == true) 
        {
            jumpTime = jumpTime - Time.deltaTime;
        }

        if (Physics.Raycast(downRay, out platformHit, 0.3f, layer))
        {
           // enemyRB.velocity = new Vector3(0, 0, 0);
           // enemyRB.angularVelocity = new Vector3(0, 0, 0);

            Debug.DrawLine(downRay.origin, downRay.origin + downRay.direction * 1, Color.green);
            if (jumpTime <= 0) 
            {
                enemyNav.enabled = true;
                jumpTime = 0.2f;
            }
        }
        else
        {
            Debug.DrawLine(downRay.origin, downRay.origin + downRay.direction * 1, Color.red);

        }
       
        void Move()
        {
            enemyNav.SetDestination(player.transform.position * speed);
        }

      
    }
    void OnTriggerEnter(Collider col)
    {
        Vector3 playerToEnemy = (transform.position - player.transform.position).normalized;

        if (col.gameObject.CompareTag("Sword"))
        {
            enemyRB.AddForce(playerToEnemy * jumpForce, ForceMode.Impulse);
            Debug.Log("Ow!");
        }
    }
}