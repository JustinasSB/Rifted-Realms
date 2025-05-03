using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    public GameObject terrainPrefab;
    public float tileSize = 1000f;
    public Transform player;

    private HashSet<Vector2Int> spawnedTiles = new HashSet<Vector2Int>();

    void Start()
    {
        SpawnTile(Vector2Int.zero);
    }
    public void SpawnTileIfNeeded(Vector2Int coord)
    {
        if (!spawnedTiles.Contains(coord))
        {
            Vector3 position = new Vector3(coord.x * tileSize, 0, coord.y * tileSize);
            position += new Vector3(-tileSize / 2f, 0, -tileSize / 2f);
            GameObject tile = Instantiate(terrainPrefab, position, Quaternion.identity);
            TerrainTile tt = tile.AddComponent<TerrainTile>();
            tt.tileCoord = coord;
            tt.spawner = this;
            tt.center = position;
            spawnedTiles.Add(coord);
        }
    }
    public bool HasTileAt(Vector2Int coord)
    {
        return spawnedTiles.Contains(coord);
    }
    private void SpawnTile(Vector2Int coord)
    {
        Vector3 position = new Vector3(coord.x * tileSize, 0, coord.y * tileSize);
        position += new Vector3(-tileSize / 2f, 0, -tileSize / 2f);
        GameObject tile = Instantiate(terrainPrefab, position, Quaternion.identity);

        TerrainTile tt = tile.AddComponent<TerrainTile>();
        tt.tileCoord = coord;
        tt.spawner = this;
        tt.center = position;
        spawnedTiles.Add(coord);
    }
}
