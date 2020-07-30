using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerAttack : MonoBehaviour
{
    public GameObject hitBox;
    public float hitTime = 2f;
    public bool hasHit = false;
    public bool hasStarted = false;
    public bool isControlling = true;

    public Vector3 playerPos;
    public Vector3 mousePos;
    public Vector3 minusPos;

    public LayerMask layer;
    RaycastHit hit;
    public int speed = 500;

    private NavMeshAgent playerAgent;

    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        playerPos = hit.point;
    }

    void Attack()
    {
        Ray cameraRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && isControlling == true)
        {
            if (Physics.Raycast(cameraRayCast, out hit, 10000, layer))
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
}
