using System.Collections.Generic;
using UnityEngine;

/*AUTHOR: Yusef*/

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<Vector3> questionTowerPositions;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private GameObject questionTowerPrefab;
    [SerializeField] private GameObject playerPrefab, monsterNPCPrefab;
    [SerializeField] private Mesh mesh;
    [SerializeField] private int monsterAmount;

    private MeshGeneratorV2 terrainGen;

    private GameObject[] monsters;

    private void Start()
    {
        terrainGen = FindObjectOfType<MeshGeneratorV2>();
        mesh = terrainGen.mesh;
        SpawnPlayer();
        SpawnQuestionTowers();

        monsters = SpawnMonsters();

        //after 5 seconds, enable the monsters
        Invoke("EnableMonsters", 5);
    }

    private void EnableMonsters()
    {
        foreach (GameObject obj in monsters)
        {
            obj.SetActive(true);
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerPosition, Quaternion.identity);
    }

    private GameObject[] SpawnMonsters()
    {
        GameObject[] monsters = new GameObject[monsterAmount];
        for (int i = 0; i < monsterAmount; i++)
        {
            int MESH_SCALE = terrainGen.MESH_SCALE;
            System.Random random = new System.Random();
            int randomVertex = random.Next(mesh.vertices.Length);
            Vector3 worldPt = transform.TransformPoint(mesh.vertices[randomVertex]);
            GameObject objectToSpawn = monsterNPCPrefab;
            monsters[i] = Instantiate(objectToSpawn, new Vector3(mesh.vertices[randomVertex].x * MESH_SCALE, mesh.vertices[randomVertex].y * MESH_SCALE, mesh.vertices[randomVertex].z * MESH_SCALE), Quaternion.identity);
            Debug.Log("spawned monster;");
        }

        return monsters;
    }

    private void SpawnQuestionTowers()
    {
        foreach (Vector3 position in questionTowerPositions)
        {
            Instantiate(questionTowerPrefab, position, Quaternion.identity);
        }
    }
}