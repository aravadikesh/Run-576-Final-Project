/* AUTHOR: ARAV*/

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    // Public variables for inspector adjustments
    public NavMeshAgent navMeshAgent;               
    public float startWaitTime = 4;                 
    public float timeToRotate = 2;                  
    public float speedWalk = 6;                     
    public float speedRun = 9;                      
    public float chaseRadius = 80;                  
    public float minSpeed = 3;                      
    public float maxSpeed = 9;                      
    public float freezeDuration = 5;               
    private bool isFrozen = false;                  
    
    // Variables for player detection and chasing
    public float viewRadius = 30;                   
    public float viewAngle = 90;                    
    public LayerMask playerMask;                   
    private bool m_playerInRange;  
    private bool player_is_chaseable = false;                 
    private Vector3 m_PlayerPosition;               
    // Variables for wandering
    public float wanderRadius = 30f;  
    public float wanderTimer = 5f;   
    private float timer;
    private bool playerInRangeNotInLOS = false;
    public float defaultFOV = 90f;
    public float alertedFOV = 120f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        timer = wanderTimer;
    }

    void Update()
    {
        if (!isFrozen)
        {
            EnvironmentView();

            if (player_is_chaseable)
            {
                AdjustFOV(alertedFOV);
                TrackPlayer();
            } 
            else if (playerInRangeNotInLOS)
            {
                AdjustFOV(alertedFOV);
                Wander();
            } else
            {
                AdjustFOV(defaultFOV);
                Wander();
            }     
        }
    }

    void EnvironmentView()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        if (colliders.Length != 0)
        {
            m_playerInRange = true;
        } else 
        {
            m_playerInRange = false;
        }

        bool playerInLOS = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            Transform target = colliders[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);


            if (angleToTarget < viewAngle * 0.5f)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dirToTarget, out hit, viewRadius, playerMask))
                {
                    playerInLOS = true;
                    m_playerInRange = true;
                    m_PlayerPosition = target.position;
                    
                }
            }
        }

        if (playerInLOS && m_playerInRange)
        {
            player_is_chaseable = true;
        } else
        {
            player_is_chaseable = false;
        }

        // Set playerInRangeNotInLOS based on LOS and proximity
        playerInRangeNotInLOS = m_playerInRange && !playerInLOS;
    }

    void AdjustFOV(float newFOV)
    {
        viewAngle = newFOV;
    }

    void TrackPlayer()
    {
        navMeshAgent.SetDestination(m_PlayerPosition);
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            EnvironmentView(); 

            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }

    void FreezeNPC()
    {
        StartCoroutine(FreezeCoroutine());
    }

    IEnumerator FreezeCoroutine()
    {
        isFrozen = true;
        Stop();
        yield return new WaitForSeconds(freezeDuration);
        isFrozen = false;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
}
