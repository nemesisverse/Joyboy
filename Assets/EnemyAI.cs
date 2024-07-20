using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaserange = 5f;

    NavMeshAgent navMeshAgent;
    float distanceTarget = Mathf.Infinity;

    bool isProvoked = false;

     // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        distanceTarget = Vector3.Distance(target.position , transform.position);
        if(isProvoked)
        {
            EngageTarget();
        }
        else if(distanceTarget <= chaserange)
        {
            isProvoked = true;
            navMeshAgent.SetDestination(target.position);
        }        
    }
    private void EngageTarget()
    {
        if(distanceTarget >= navMeshAgent.stoppingDistance)
        {
            chasetarget();

        }


        if(distanceTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void chasetarget()
    {
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        Debug.Log("is attacking");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , chaserange);
    }

    void LookMouse()
    {
        Vector2 directionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(directionMouse.y, directionMouse.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = rotation;
    }
}
