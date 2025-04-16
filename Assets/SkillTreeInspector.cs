using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeInspector : MonoBehaviour
{
    public static SkillTreeInspector instance;
    public static int AllocatedNodesCounter = 0;
    [SerializeField]
    GameObject Panel;
    [SerializeField]
    GameObject Nodes;
    bool isVisible;
    [SerializeField]
    SkillTree Root;
    float scale = 1;
    public float ZoomRate = 0.05f;
    public bool GetValidity()
    {
        HashSet<Guid> traversedSet = new HashSet<Guid>();
        DFS(Root, traversedSet);
        if (traversedSet.Count == AllocatedNodesCounter) return true;
        return false;
    }
    public void DFS(SkillTree Node, HashSet<Guid> traversed)
    {
        foreach (SkillTree node in Node.Connections)
        {
            if (traversed.Contains(node.Id)) continue;
            traversed.Add(node.Id);
            DFS(node, traversed);
        }
    }
    private void Start()
    {
        instance = this;
        isVisible = instance.Panel.activeSelf;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isVisible = !isVisible;
            Panel.SetActive(isVisible);
        }
        if (!isVisible) return;
        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0) return;
        scale = Mathf.Clamp(scale + ZoomRate * scroll, 0.3f, 1f);
        Nodes.transform.localScale = new Vector3 (scale,scale);
    }
}