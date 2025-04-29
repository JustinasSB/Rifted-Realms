using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.UI;

public class PassiveTreeManager : MonoBehaviour, IUIToggleable
{
    public static PassiveTreeManager instance;
    public static int AllocatedNodesCounter = 0;
    [SerializeField]
    GameObject Panel;
    [SerializeField]
    GameObject Nodes;
    public bool isVisible;
    [SerializeField]
    public PassiveTreeNode Root;
    float scale = 1;
    public float ZoomRate = 0.05f;
    [SerializeField] private GameObject linePrefab;
    // Added in order to hide when PassiveTreeCanvas is toggled
    [SerializeField] GameObject mana;
    [SerializeField] GameObject experience;
    Dictionary<(PassiveTreeNode, PassiveTreeNode), Image> lines = new();
    public bool GetValidity()
    {
        HashSet<Guid> traversedSet = new HashSet<Guid>();
        DFS(Root, traversedSet);
        if (traversedSet.Count == AllocatedNodesCounter) return true;
        return false;
    }
    public void DFS(PassiveTreeNode Node, HashSet<Guid> traversed)
    {
        foreach (PassiveTreeNode node in Node.Connections)
        {
            if (traversed.Contains(node.Id) || node.Equals(Root)) continue;
            traversed.Add(node.Id);
            DFS(node, traversed);
        }
    }
    public void Toggle()
    {
        isVisible = !isVisible;
        Panel.SetActive(isVisible);
        foreach (PassiveTreeAbilityNode node in Nodes.GetComponentsInChildren<PassiveTreeAbilityNode>())
        {
            node.DestroyTooltip();
        }
        if (!isVisible)
        {
            mana.SetActive(true);
            experience.SetActive(true);
        }
        else
        {
            mana.SetActive(false);
            experience.SetActive(false);
        }
    }
    public bool IsOpen => isVisible;
    private void Start()
    {
        instance = this;
        isVisible = instance.Panel.activeSelf;
        drawLines();
        Toggle();
    }
    private void Update()
    {
        if (!isVisible) return;
        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0) return;
        scale = Mathf.Clamp(scale + ZoomRate * scroll, 0.3f, 1f);
        Nodes.transform.localScale = new Vector3 (scale,scale);
    }
    private void drawLines()
    {
        if (Root == null) return;
        HashSet<(PassiveTreeNode, PassiveTreeNode)> drawnConnections = new();
        BFS(drawnConnections);
        foreach (var (a, b) in drawnConnections)
        {
            DrawLineBetween(a, b);
        }
        SetColors();
    }
    private void BFS(HashSet<(PassiveTreeNode, PassiveTreeNode)> drawnConnections) 
    {
        if (this.Root == null) return; 
        Queue<PassiveTreeNode> nodesToExplore = new();
        HashSet<Guid> explored = new();
        nodesToExplore.Enqueue(this.Root);
        explored.Add(this.Root.Id);
        while (nodesToExplore.Count > 0)
        {
            var current = nodesToExplore.Dequeue();

            foreach (var neighbor in current.Neighbors)
            {
                if (neighbor == null)
                    continue;

                if (explored.Add(neighbor.Id))
                {
                    nodesToExplore.Enqueue(neighbor);
                }

                var pairKey = MakePairKey(current, neighbor);
                if (!drawnConnections.Contains(pairKey))
                {
                    drawnConnections.Add(pairKey);
                }
            }
        }
    }
    private void DrawLineBetween(PassiveTreeNode NodeA, PassiveTreeNode NodeB)
    {
        Vector3 start = NodeA.transform.position;
        Vector3 end = NodeB.transform.position;
        Vector3 direction = end - start;
        Vector3 midPoint = (start + end) / 2f;
        float length = direction.magnitude;

        GameObject line = Instantiate(linePrefab, midPoint, Quaternion.identity, Nodes.transform);
        line.transform.SetSiblingIndex(0);
        line.transform.right = direction.normalized;
        line.transform.localScale = new Vector3(length, line.transform.localScale.y, line.transform.localScale.z);
        lines.Add((NodeA, NodeB), line.GetComponent<Image>());
    }
    private (PassiveTreeNode, PassiveTreeNode) MakePairKey(PassiveTreeNode a, PassiveTreeNode b)
    {
        return a.CompareTo(b) < 0 ? (a, b) : (b, a);
    }
    private void SetColors()
    {
        foreach (KeyValuePair<(PassiveTreeNode, PassiveTreeNode), Image> line in lines)
        {
            int count = (line.Key.Item1.Allocated ? 1 : 0) + (line.Key.Item2.Allocated ? 1 : 0);
            switch (count)
            {
                case 0:
                    line.Value.color = new Color32(87, 72, 54, 255);
                    break;
                case 1:
                    line.Value.color = new Color32(137, 117, 92, 255);
                    break;
                case 2:
                    line.Value.color = new Color32(154, 132, 104, 255);
                    break;
            }
        }
    }
    public void SetColors((PassiveTreeNode, PassiveTreeNode) key, int type)
    {
        switch (type)
        {
            case 0:
                lines[key].color = new Color32(87, 72, 54, 255);
                break;
            case 1:
                lines[key].color = new Color32(137, 117, 92, 255);
                break;
            case 2:
                lines[key].color = new Color32(154, 132, 104, 255);
                break;
        }
    }
}

