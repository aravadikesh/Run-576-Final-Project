// Code written by Arav Adikesh Ramakrishnan

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;


[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorV2 : MonoBehaviour
{
    public Mesh mesh;
    public int MESH_SCALE = 10;
    public AnimationCurve heightCurve;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
    [SerializeField] private Gradient gradient;
    public GameObject[] objects;
    private float minTerrainheight;
    private float maxTerrainheight;
    private float lastNoiseHeight;
    public int xSize;
    public int zSize;
    public float scale; 
    public int octaves;
    public float lacunarity;
    public int seed;
    private System.Random prng;
    private Vector2[] octaveOffsets;
    [SerializeField] private Material material;
    [SerializeField] private MeshRenderer meshRenderer;
    public GameObject questionStationPrefab;   // Prefab for question station
    public GameObject monsterNPCPrefab;        // Prefab for chasing NPC
    public int numberOfQuestionStations = 5;   // Number of question stations to spawn
    private float minimumDistanceBetweenStations = 10;  // Constraint for question station spacing

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateNewMap();

        // Spawn question stations and monster NPC
        //SpawnQuestionStations();
        //SpawnMonsterNPC();
    }

    public void CreateNewMap()
    {
        CreateMeshShape();
        CreateTriangles();
        ColorMap();
        UpdateMesh();
    }

    private void CreateMeshShape()
    {
        // Creates seed
        Vector2[] octaveOffsets = GetOffsetSeed();

        if (scale <= 0) scale = 0.0001f;

        // Generate noise map
        float[,] noiseMap = Noise.GenerateNoiseMap(xSize + 1, zSize + 1, new NoiseSettings
        {
            normalizeMode = Noise.NormalizeMode.Global,
            scale = scale,
            octaves = octaves,
            persistance = 0.5f,
            lacunarity = lacunarity,
            seed = seed,
            offset = octaveOffsets[0] 
        }, Vector2.zero);

        // Create vertices
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Set height of vertices from noise map, scaling and offsetting the values
                float noiseHeight = noiseMap[x, z] * MESH_SCALE; 
                SetMinMaxHeights(noiseHeight);
                vertices[i] = new Vector3(x, noiseHeight, z);
                i++;
            }
        }
    }
    
    private Vector2[] GetOffsetSeed()
    {
        seed = Random.Range(0, 1000);
        // changes area of map
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int o = 0; o < octaves; o++)
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[o] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }

    private void SetMinMaxHeights(float noiseHeight)
    {
        if (noiseHeight > maxTerrainheight)
            maxTerrainheight = noiseHeight;
        if (noiseHeight < minTerrainheight)
            minTerrainheight = noiseHeight;
    }

    private void CreateTriangles()
    {
        // Need 6 vertices to create a square (2 triangles)
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        // loop through rows
        for (int z = 0; z < xSize; z++)
        {
            // fill all columns in row
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void ColorMap()
    {
        colors = new Color[vertices.Length];

        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                int index = z * (xSize + 1) + x;
                float height = Mathf.InverseLerp(minTerrainheight, maxTerrainheight, vertices[index].y);
                colors[index] = gradient.Evaluate(height);
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.transform.localScale = new Vector3(MESH_SCALE, MESH_SCALE, MESH_SCALE);

        SpawnObjects();
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void SpawnObjects() 
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            // find actual position of vertices in the game
            Vector3 worldPt = transform.TransformPoint(mesh.vertices[i]);
            var noiseHeight = worldPt.y;
            // Stop generation if height difference between 2 vertices is too steep
            if(System.Math.Abs(lastNoiseHeight - worldPt.y) < MESH_SCALE)
            {
                // min height for object generation
                if (noiseHeight > 30)
                {
                    // Chance to generate
                    if (Random.Range(1, 2) == 1)
                    {
                        GameObject objectToSpawn = objects[Random.Range(0, objects.Length)];
                        //var spawnAboveTerrainBy = noiseHeight * 2;
                        Instantiate(objectToSpawn, new Vector3(mesh.vertices[i].x * MESH_SCALE, mesh.vertices[i].y * MESH_SCALE, mesh.vertices[i].z * MESH_SCALE), Quaternion.identity);
                    }
                }
            }
            lastNoiseHeight = noiseHeight;
        }
    }
}
