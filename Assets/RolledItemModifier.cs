using UnityEngine;

public class RolledItemModifier
{
    public ItemModifier Template { get; }
    public float Value { get; private set; }

    public string Description => $"+{Value} {Template.AffectedStat}";

    public RolledItemModifier(ItemModifier template)
    {
        Template = template;
        Value = UnityEngine.Random.Range(template.RollRangeMin, template.RollRangeMax);
    }
    public void Reroll()
    {
        Value = UnityEngine.Random.Range(Template.RollRangeMin, Template.RollRangeMax);
    }
}
