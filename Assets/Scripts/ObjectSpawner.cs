using System.Collections.Generic;
using UnityEngine;

/*AUTHOR: Yusef*/

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<Vector3> questionTowerPositions;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private GameObject questionTowerPrefab;
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        SpawnPlayer();
        SpawnQuestionTowers();
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerPosition, Quaternion.identity);
    }

    private void SpawnQuestionTowers()
    {
        foreach (Vector3 position in questionTowerPositions)
        {
            Instantiate(questionTowerPrefab, position, Quaternion.identity);
        }
    }
}