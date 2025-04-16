using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class SkillTree : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public Guid Id = Guid.NewGuid();
    public string Name;
    public string Description;
    public StatType To;
    public OperationType OperationType;
    public StatType From;
    public float Value;
    public bool Allocated;
    public List<SkillTree> Neighbors = new();
    public List<SkillTree> Connections = new();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (LevelManager.level.SkillPoints <= 0) return;
        if (!this.Allocated) allocateNode();
        else deallocateNode();
    }
    private void allocateNode()
    {
        bool firstConnection = true;
        foreach (SkillTree neighbor in Neighbors)
        {
            if (neighbor.Allocated == false) continue;
            if (firstConnection)
            {
                Allocated = true;
                SkillTreeInspector.AllocatedNodesCounter++;
                LevelManager.level.SkillPoints--;
                addModifier();
                firstConnection = false;
            }
            neighbor.AddConnection(this);
            this.AddConnection(neighbor);
        }
    }
    private void deallocateNode()
    {
        Allocated = false;
        SkillTreeInspector.AllocatedNodesCounter--;
        RemoveAllConnections();
        if (Connections.Count < 2 
            || SkillTreeInspector.instance.GetValidity())
        {
            removeModifier();
            Connections.Clear();
            LevelManager.level.SkillPoints++;
            return;
        }
        Allocated = true;
        RestoreConnections();
    }
    public void RestoreConnections()
    {
        foreach (SkillTree connection in Connections)
        {
            connection.AddConnection(this);
        }
    }
    private void AddConnection(SkillTree node)
    {
        this.Connections.Add(node);
    }
    public void RemoveAllConnections()
    {
        foreach (SkillTree connection in Connections)
        {
            connection.RemoveConnection(this);
        }
    }
    private void RemoveConnection(SkillTree node)
    {
        this.Connections.Remove(node);
    }
    private void addModifier()
    {
        if (OperationType == OperationType.Extra || OperationType == OperationType.Convert)
            PlayerStatsManager.playerStats.ModifyStat(To, OperationType, Value, From);
        else PlayerStatsManager.playerStats.ModifyStat(To, OperationType, Value);
    }
    private void removeModifier()
    {
        switch (OperationType)
        {
            case OperationType.Add:
                PlayerStatsManager.playerStats.ModifyStat(To, OperationType.AddRemove, Value);
                break;
            case OperationType.Increase:
                PlayerStatsManager.playerStats.ModifyStat(To, OperationType.IncreaseRemove, Value);
                break;
            case OperationType.Multiply:
                PlayerStatsManager.playerStats.ModifyStat(To, OperationType.MultiplyRemove, Value);
                break;
            case OperationType.Convert:
                PlayerStatsManager.playerStats.ModifyStat(To, OperationType.ConvertRemove, Value, From);
                break;
            case OperationType.Extra:
                PlayerStatsManager.playerStats.ModifyStat(To, OperationType.ExtraRemove, Value, From);
                break;
            case OperationType.SetBase:
                PlayerStatsManager.playerStats.ModifyStat(To, OperationType.SetBase, Value);
                break;
            default:
                break;
        }
    }
}