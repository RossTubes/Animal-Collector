using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animals : MonoBehaviour
{
    public Transform player;
    public float wanderRadius;
    public float wanderTimer;
    public float fleeDistance;
    public float sightDistance;
    public float normalSpeed;
    public float fleeSpeed;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        agent.speed = normalSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < fleeDistance)
        {
            Flee();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        agent.speed = normalSpeed;
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    void Flee()
    {
        agent.speed = fleeSpeed;

        // Calculate the flee direction away from the player
        Vector3 fleeDirection = (transform.position - player.position).normalized;

        // Set the flee position at a fixed distance in the opposite direction from the player
        Vector3 fleePosition = transform.position + fleeDirection * fleeDistance;

        // Set the destination to the calculated flee position
        agent.SetDestination(fleePosition);
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightDistance);
    }
}
