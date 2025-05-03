using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    public Vector2Int tileCoord;
    public TerrainSpawner spawner;
    private List<(Vector3, Vector2Int)> edges = new();
    private List<(Vector3, Vector2Int)> scheduleRemove = new();
    private float size;
    private float distance;
    public Vector3 center;
    float projectionThreshold;
    float perpendicularThreshold;

    void Update()
    {
        if (edges.Count == 0) return;
        foreach (var edge in edges)
        {
            CheckEdge(edge);
        }
        foreach (var edge in scheduleRemove)
        {
            foreach (var e in edges)
            {
                if (edge.Item2 == e.Item2)
                {
                    edges.Remove(e);
                    break;
                }
            }
        }
        scheduleRemove.Clear();
    }
    private void Start()
    {
        size = spawner.tileSize;
        distance = size * 0.75f;
        float half = size / 2f;
        projectionThreshold = size * 0.02f;
        perpendicularThreshold= size * 1f;
        TryAddEdge(Vector2Int.right, center + new Vector3(size, 0, size / 2f));
        TryAddEdge(Vector2Int.left, center + new Vector3(0, 0, size / 2f));
        TryAddEdge(Vector2Int.up, center + new Vector3(size / 2f, 0, size));
        TryAddEdge(Vector2Int.down, center + new Vector3(size / 2f, 0, 0));
    }
    private void TryAddEdge(Vector2Int direction, Vector3 worldEdge)
    {
        Vector2Int neighborCoord = tileCoord + direction;
        if (!spawner.HasTileAt(neighborCoord))
        {
            edges.Add((worldEdge, direction));
        }
    }

    void CheckEdge((Vector3, Vector2Int) edge)
    {
        Vector3 playerPos = spawner.player.position;
        Vector3 dirToEdge = (edge.Item1 - center).normalized;
        float projection = Vector3.Dot(playerPos - center, dirToEdge);
        float perpendicularDistance = Vector3.Cross(playerPos - center, dirToEdge).magnitude;

        if (projection > projectionThreshold && perpendicularDistance < perpendicularThreshold)
        {
            spawner.SpawnTileIfNeeded(tileCoord + edge.Item2);
            scheduleRemove.Add(edge);
        }
    }
    void OnDrawGizmos()
    {
        if (edges == null) return;

        Gizmos.color = Color.red;
        foreach (var edge in edges)
        {
            Gizmos.DrawSphere(edge.Item1 + Vector3.up * 2f, 2f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center + Vector3.up * 0.1f, new Vector3(size, 0.1f, size));
    }
}
