using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent enemyAgent;
    void Start() 
    {
        target = PlayerManager.instance.player.transform;
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    void Update() 
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius) 
        {
            enemyAgent.SetDestination(target.position);

            if (distance <= enemyAgent.stoppingDistance) 
            {
                FaceTarget();
            }
        }
    }

    void FaceTarget() 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}