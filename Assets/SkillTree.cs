using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

//probably not scriptable object
//mono behaviour??
//what the hell is a skill tree
public class SkillTree
{
    [HideInInspector]
    public Guid Id = Guid.NewGuid();
    public string Name { get; set; }
    public string Description { get; set; }
    public StatType To { get; set; }
    public OperationType OperationType { get; set; }
    public StatType From { get; set; }
    public float value { get; set; }
    public List<SkillTree> Connectables { get; set; } = new();
    public List<SkillTree> Connections { get; set; } = new();
}
