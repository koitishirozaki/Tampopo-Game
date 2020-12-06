using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class AI_SimpleFollow : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    
    public float range;
    private GameObject target;
    public LayerMask objectMask;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        SimpleFollow(target);
    }

    void SimpleFollow(GameObject thething)
    {
        navMeshAgent.SetDestination(thething.transform.position);
    }

    void CheckObjectsAround()
    {
        Collider[] objectsAround = Physics.OverlapSphere(transform.position, 360, objectMask);
        
        for(int i = 0; i< objectsAround.Length; i++)
        {
            Transform target = objectsAround[i].transform;
            Vector3 dirToTarget = target.position - transform.position;

            if(dirToTarget.magnitude <= range)
            {
            // dont know how to make them distanced tho
            }

        }
    }

}
