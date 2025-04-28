using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] Terrain terrain;
    [SerializeField] TerrainCollider terrainCollider;
    [SerializeField] float scale = 0.05f;
    [SerializeField] float heightMultiplier = 0.1f;
    [Header("Grass and Tree Prefabs")]
    [SerializeField] GameObject[] treePrefabs;
    [Header("Spawn Settings")]
    [SerializeField] private float treeSpawnProbability = 0.05f;
    [SerializeField] private float treeSpacing = 4f;
    [SerializeField] private float minTreeScale = 0.8f;
    [SerializeField] private float maxTreeScale = 1.5f;
    [Header("Tile Map Settings")]
    [SerializeField] int mapWidth = 10;
    [SerializeField] int mapHeight = 10;
    [SerializeField] float tileSize = 2f;
    private int[,] tileMap;
    private Dictionary<int, List<GameObject>> tileTreeBuckets = new Dictionary<int, List<GameObject>>();
    // Define allowed adjacent tile values.
    // Example, if a tile is type 1, allowed adjacent tiles will be values 1 or 2.
    private Dictionary<int, List<int>> allowedAdjacents = new Dictionary<int, List<int>>() {
        { 1, new List<int> { 1, 2, 3 } },
        { 2, new List<int> { 1, 2 } },
        { 3, new List<int> { 1, 3 } }
    };
    void Start()
    {
        tileTreeBuckets.Add(1, new List<GameObject>());
        tileTreeBuckets.Add(2, new List<GameObject>());
        tileTreeBuckets.Add(3, new List<GameObject>());
        if (treePrefabs.Count() > 0)
        {
            tileTreeBuckets[1].Add(treePrefabs[0]);
            tileTreeBuckets[1].Add(treePrefabs[3]);
            tileTreeBuckets[1].Add(treePrefabs[4]);
            tileTreeBuckets[2].Add(treePrefabs[1]);
            tileTreeBuckets[3].Add(treePrefabs[2]);
        }
        tileMap = new int[mapWidth, mapHeight];
        tileSize = terrain.terrainData.size.x / mapWidth;
        GenerateTileMap();
        TerrainData originalData = terrain.terrainData;
        TerrainData newData = Instantiate(originalData);
        terrain.terrainData = newData;
        terrainCollider.terrainData = newData;
        GenerateTerrain(newData);
        AddGrassAndTrees(newData);
    }
    void GenerateTileMap()
    {
        List<int> allTileValues = new List<int>() { 1, 2, 3};
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                List<int> possibleValues = new List<int>(allTileValues);
                if (x > 0)
                {
                    int leftValue = tileMap[x - 1, y];
                    possibleValues = Intersect(possibleValues, allowedAdjacents[leftValue]);
                }
                if (y > 0)
                {
                    int bottomValue = tileMap[x, y - 1];
                    possibleValues = Intersect(possibleValues, allowedAdjacents[bottomValue]);
                }
                if (possibleValues.Count == 0)
                    possibleValues.Add(1);
                tileMap[x, y] = possibleValues[Random.Range(0, possibleValues.Count)];
            }
        }
    }
    List<int> Intersect(List<int> listA, List<int> listB)
    {
        List<int> result = new List<int>();
        foreach (int a in listA)
        {
            if (listB.Contains(a))
                result.Add(a);
        }
        return result;
    }
    void GenerateTerrain(TerrainData terrainData)
    {
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;

        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = x * scale;
                float yCoord = y * scale;
                heights[x, y] = Mathf.PerlinNoise(xCoord, yCoord) * heightMultiplier;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
    void AddGrassAndTrees(TerrainData terrainData)
    {
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPos = terrain.transform.position;

        if (treePrefabs != null && treePrefabs.Length > 0)
        {
            for (float x = 0; x < terrainSize.x; x += treeSpacing)
            {
                for (float z = 0; z < terrainSize.z; z += treeSpacing)
                {
                    int tileX = Mathf.FloorToInt(x / tileSize);
                    int tileY = Mathf.FloorToInt(z / tileSize);

                    if (tileX >= mapWidth || tileY >= mapHeight)
                        continue;

                    int tileType = tileMap[tileX, tileY];

                    if (!tileTreeBuckets.ContainsKey(tileType) || tileTreeBuckets[tileType].Count == 0)
                        continue;

                    if (Random.Range(0f, 1f) < treeSpawnProbability)
                    {
                        Vector3 worldPos = new Vector3(x, 0, z) + terrainPos;
                        float y = terrain.SampleHeight(worldPos);
                        worldPos.y = y + terrainPos.y;
                        List<GameObject> bucket = tileTreeBuckets[tileType];
                        GameObject treePrefab = bucket[Random.Range(0, bucket.Count)];

                        GameObject treeInstance = Instantiate(
                            treePrefab,
                            worldPos,
                            Quaternion.Euler(0, Random.Range(-360f, 360f), 0),
                            transform);
                        float treeScale = Random.Range(minTreeScale, maxTreeScale);
                        treeInstance.transform.localScale = Vector3.one * treeScale;
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (tileMap == null)
            return;
        Vector3 terrainPos = (terrain != null) ? terrain.transform.position : Vector3.zero;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int tileType = tileMap[x, y];
                Color gizmoColor = Color.white;
                switch (tileType)
                {
                    case 1:
                        gizmoColor = Color.red;
                        break;
                    case 2:
                        gizmoColor = Color.green;
                        break;
                    case 3:
                        gizmoColor = Color.blue;
                        break;
                }
                Gizmos.color = gizmoColor;
                Vector3 pos = terrainPos + new Vector3(x * tileSize, 0, y * tileSize) + new Vector3(tileSize * 0.5f, 0, tileSize * 0.5f);
                Gizmos.DrawCube(pos, new Vector3(tileSize, 0.1f, tileSize));
            }
        }
    }
}
