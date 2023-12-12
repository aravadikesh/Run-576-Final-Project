// Code written by Arav Adikesh Ramakrishnan
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    // Public variables for inspector adjustments
    public NavMeshAgent navMeshAgent;               // Reference to the NavMeshAgent component
    public float startWaitTime = 4;                 // Initial wait time before starting any action
    public float timeToRotate = 2;                  // Wait time when the enemy detects the player without visual contact
    public float speedWalk = 6;                     // Walking speed
    public float speedRun = 9;                      // Running speed
    public float chaseRadius = 40;                  // Radius to trigger faster chasing
    public float minSpeed = 3;                      // Minimum speed when far from player
    public float maxSpeed = 9;                      // Maximum speed when close to player
    public float freezeDuration = 5;               // Duration to freeze NPC after achieving an objective
    private bool isFrozen = false;                  // Flag to check if NPC is frozen

    // Variables for player detection and chasing
    public float viewRadius = 30;                   // Radius of the enemy's field of vision
    public float viewAngle = 90;                    // Angle of the enemy's field of vision
    public LayerMask playerMask;                    // Layer mask to detect the player
    public float meshResolution = 1.0f;             // Number of rays cast per degree for environment scanning
    public int edgeIterations = 4;                  // Number of iterations to enhance mesh filtering when rays hit obstacles
    public float edgeDistance = 0.5f;               // Maximum distance for raycasting when an obstacle is hit
    
    // Variables for player detection and chasing
    private bool m_playerInRange;                   // If the player is in range of vision, state of chasing
    private Vector3 m_PlayerPosition;               // Last position of the player when the player is seen by the enemy

    void Start()
    {
        // Initialize NavMeshAgent and set initial properties
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
    }

    void Update()
    {
        // Check if the NPC is not frozen before updating its behavior
        if (!isFrozen)
        {
            // Directly track the player
            EnvironmentView();
            TrackPlayer();
        }
    }


    // Function to perform environment scanning to detect the player
    void EnvironmentView()
    {
        // Detect all colliders within the view radius and player layer
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        // Iterate through all detected colliders
        for (int i = 0; i < playerInRange.Length; i++)
        {
            // Get the player's transform and direction to the player
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            // Calculate the distance to the player
            float dstToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the chase radius
            if (dstToPlayer <= chaseRadius)
            {
                m_playerInRange = true;
                m_PlayerPosition = player.transform.position;
            }
            else
            {
                m_playerInRange = false;
            }
        }
    }

    // Function to handle tracking the player
    void TrackPlayer()
    {
        if (m_playerInRange) // Check if the player is within the NPC's detection range
        {
            // Move the NPC with speed based on the distance to the player
            Move(Mathf.Lerp(minSpeed, maxSpeed, Vector3.Distance(transform.position, m_PlayerPosition) / chaseRadius));
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        else
        {
            // Rotate the NPC in a scanning circle
            RotateInCircle();
        }
    }

    // Function to rotate the NPC in a scanning circle
    void RotateInCircle()
    {
        float rotationSpeed = 20f; // Adjust this to control the rotation speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


    // Function to freeze the NPC for a specified duration
    void FreezeNPC()
    {
        StartCoroutine(FreezeCoroutine());
    }

    // Coroutine to handle freezing the NPC
    IEnumerator FreezeCoroutine()
    {
        // Set the frozen flag to true and stop the NPC
        isFrozen = true;
        Stop();
        // Wait for the specified duration
        yield return new WaitForSeconds(freezeDuration);
        // Set the frozen flag to false after the duration
        isFrozen = false;
    }

    // Function to stop the NPC
    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    // Function to move the NPC with a specified speed
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
}
