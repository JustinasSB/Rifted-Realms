using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveTreeNode : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public Guid Id = Guid.NewGuid();
    public string Name;
    [SerializeField] public bool special;
    public string Description;
    [SerializeField] List<StatModifier> Modifiers;
    public bool Allocated;
    public List<PassiveTreeNode> Neighbors = new();
    public List<PassiveTreeNode> Connections = new();

    [SerializeField] private Image Border;
    [SerializeField] private Image Background;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (this.Allocated) { deallocateNode(); return; }
        if (LevelManager.level.SkillPoints <= 0) return;
        else allocateNode();
    }
    void Start()
    {
        if (!special)
        {
            Description = "";
            int count = Modifiers.Count;
            foreach (StatModifier modifier in Modifiers)
            {
                int negative = 0;
                if (modifier.OperationType == OperationType.Multiply)
                {
                    negative = modifier.Value < 100 ? 1 : 0;
                }
                else negative = modifier.Value < 0 ? 1 : 0;
                switch ((int)modifier.OperationType + 10 * negative)
                {
                    case 0:
                        Description += $"+{modifier.Value} To {modifier.To.GetDisplayName()}";
                        break;
                    case 1:
                        Description += $"{modifier.To.GetDisplayName()} increased by +{modifier.Value}%";
                        break;
                    case 2:
                        Description += $"{modifier.Value - 100}% More {modifier.To.GetDisplayName()}";
                        break;
                    case 3:
                        Description += $"{modifier.Value}% of {modifier.From.GetDisplayName()} added as {modifier.To.GetDisplayName()}";
                        break;
                    case 4:
                        Description += $"{modifier.Value}% of {modifier.From.GetDisplayName()} added as extra {modifier.To.GetDisplayName()}";
                        break;
                    case 10:
                        Description += $"{modifier.Value} To {modifier.To.GetDisplayName()}";
                        break;
                    case 11:
                        Description += $"{Math.Abs(modifier.Value)}% Reduction To {modifier.To.GetDisplayName()}";
                        break;
                    case 12:
                        Description += $"{Math.Abs(100 - modifier.Value)}% Less {modifier.To.GetDisplayName()}";
                        break;
                }
                if (count > 1)
                {
                    count--;
                    Description += "\n";
                }
            }
        }
    }
    public void allocateNode()
    {
        bool firstConnection = true;
        foreach (PassiveTreeNode neighbor in Neighbors)
        {
            if (neighbor.Allocated == false) continue;
            if (firstConnection)
            {
                Allocated = true;
                PassiveTreeManager.AllocatedNodesCounter++;
                LevelManager.level.DecrementPassivePoints();
                addModifier();
                highLightNode();
                setLineColors();
                firstConnection = false;
            }
            neighbor.AddConnection(this);
            this.AddConnection(neighbor);
        }
    }
    public void deallocateNode()
    {
        Allocated = false;
        RemoveAllConnections();
        PassiveTreeManager.AllocatedNodesCounter--;
        if (Connections.Count < 2 
            || PassiveTreeManager.instance.GetValidity())
        {
            removeModifier();
            Connections.Clear();
            LevelManager.level.IncrementPassivePoints();
            highLightNode();
            setLineColors();
            return;
        }
        Allocated = true;
        RestoreConnections();
        PassiveTreeManager.AllocatedNodesCounter++;
    }
    public void RestoreConnections()
    {
        foreach (PassiveTreeNode connection in Connections)
        {
            connection.AddConnection(this);
        }
    }
    private void AddConnection(PassiveTreeNode node)
    {
        this.Connections.Add(node);
    }
    public void RemoveAllConnections()
    {
        foreach (PassiveTreeNode connection in Connections)
        {
            connection.RemoveConnection(this);
        }
    }
    private void RemoveConnection(PassiveTreeNode node)
    {
        this.Connections.Remove(node);
    }
    private void addModifier()
    {
        foreach (StatModifier modifier in Modifiers)
        {
            if (modifier.OperationType == OperationType.Extra || modifier.OperationType == OperationType.Convert)
                PlayerStatsManager.playerStats.ModifyStat(modifier.To, modifier.OperationType, modifier.Value / 100, modifier.From);
            else if (modifier.OperationType == OperationType.Add || modifier.OperationType == OperationType.SetBase)
                PlayerStatsManager.playerStats.ModifyStat(modifier.To, modifier.OperationType, modifier.Value);
            else
                PlayerStatsManager.playerStats.ModifyStat(modifier.To, modifier.OperationType, modifier.Value / 100);
        }
    }
    private void removeModifier()
    {
        foreach (StatModifier modifier in Modifiers)
        {
            switch (modifier.OperationType)
            {
                case OperationType.Add:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.To, OperationType.AddRemove, modifier.Value);
                    break;
                case OperationType.Increase:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.To, OperationType.IncreaseRemove, modifier.Value / 100);
                    break;
                case OperationType.Multiply:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.To, OperationType.MultiplyRemove, modifier.Value / 100);
                    break;
                case OperationType.Convert:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.To, OperationType.ConvertRemove, modifier.Value / 100, modifier.From);
                    break;
                case OperationType.Extra:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.To, OperationType.ExtraRemove, modifier.Value / 100, modifier.From);
                    break;
                case OperationType.SetBase:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.To, OperationType.SetBase, modifier.Value);
                    break;
                default:
                    break;
            }
        }
    }
    private void highLightNode()
    {
        if (Allocated) {
            Border.color = Color.white;
            Background.color = Color.white;
        }
        else 
        {
            Border.color = new Color32(71, 71, 71, 255);
            Background.color = new Color32(57, 57, 57, 255);
        }
    }
    public void highLightNode(bool hightlight)
    {
        if (hightlight)
        {
            Border.color = Color.white;
            Background.color = Color.white;
        }
        else
        {
            Border.color = new Color32(71, 71, 71, 255);
            Background.color = new Color32(57, 57, 57, 255);
        }
    }
    private void setLineColors()
    {
        int a = this.Allocated ? 1 : 0;
        foreach (var neighbor in Neighbors)
        {
            int count = a + (neighbor.Allocated ? 1 : 0);
            PassiveTreeManager.instance.SetColors(MakePairKey(this, neighbor), count);
        }
    }
    private (PassiveTreeNode, PassiveTreeNode) MakePairKey(PassiveTreeNode a, PassiveTreeNode b)
    {
        return a.CompareTo(b) < 0 ? (a, b) : (b, a);
    }
    internal int CompareTo(PassiveTreeNode b)
    {
        return this.Id.ToString().CompareTo(b.Id.ToString());
    }
}
public class PassiveTooltipData
{
    public Guid Id;
    public string Name;
    public string Description;

    public PassiveTooltipData(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}