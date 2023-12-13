using UnityEngine;
using UnityEngine.AI;

public class SpeedUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    // Update is called once per frame
    void Update()
    {
        agent.speed = GameManager.Instance.enemySpeed;
    }
}