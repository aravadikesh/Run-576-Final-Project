/*AUTHOR: ARAV*/

using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;
    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1.0f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;
    [SerializeField] public Transform[] waypoints;
    private int currentWaypointIndex;
    private Vector3 lastKnownPlayerPosition;
    private float waitTime;
    private float timeToRotatePlayerNear;
    private bool isPatrolling = true;
    private bool caughtPlayer;
    private Vector3 m_PlayerPosition;
    private bool m_PlayerNear;

    void Start()
    {
        currentWaypointIndex = 0;
        waitTime = startWaitTime;
        timeToRotatePlayerNear = timeToRotate;
        navMeshAgent = GetComponent<NavMeshAgent>();
        Move(speedWalk, waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        EnvironmentView();

        if (!isPatrolling)
            Chasing();
        else
            Patrolling();
    }

    void Chasing()
    {
        Move(speedRun, m_PlayerPosition);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (waitTime <= 0 && !caughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
                ReturnToPatrol();
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    Stop();
                waitTime -= Time.deltaTime;
            }
        }
    }

    void Patrolling()
    {
        if (m_PlayerNear)
            CheckPlayerNear();
        else
            CheckWaypoint();

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (waitTime <= 0)
            {
                if (m_PlayerNear)
                    ReturnToPatrol();
                else
                    NextWaypoint();
            }
            else
            {
                Stop();
                waitTime -= Time.deltaTime;
            }
        }
    }

    void ReturnToPatrol()
    {
        isPatrolling = true;
        m_PlayerNear = false;
        Move(speedWalk, waypoints[currentWaypointIndex].position);
        timeToRotatePlayerNear = timeToRotate;
        waitTime = startWaitTime;
    }

    void CheckPlayerNear()
    {
        if (timeToRotatePlayerNear <= 0)
        {
            Move(speedWalk, lastKnownPlayerPosition);
        }
        else
        {
            Stop();
            timeToRotatePlayerNear -= Time.deltaTime;
        }
    }

    void CheckWaypoint()
    {
        m_PlayerNear = false;
        lastKnownPlayerPosition = Vector3.zero;
        Move(speedWalk, waypoints[currentWaypointIndex].position);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (waitTime <= 0)
            {
                NextWaypoint();
                waitTime = startWaitTime;
            }
            else
            {
                Stop();
                waitTime -= Time.deltaTime;
            }
        }
    }

    void NextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        Move(speedWalk, waypoints[currentWaypointIndex].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed, Vector3 destination)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
        navMeshAgent.SetDestination(destination);
    }

    void CaughtPlayer()
    {
        caughtPlayer = true;
    }

    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerNear = true;
                    isPatrolling = false;
                }
                else
                    m_PlayerNear = false;
            }

            if (Vector3.Distance(transform.position, player.position) > viewRadius)
                m_PlayerNear = false;

            if (m_PlayerNear)
                m_PlayerPosition = player.transform.position;
        }
    }
}
