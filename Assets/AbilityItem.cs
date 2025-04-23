using UnityEngine;

[CreateAssetMenu(fileName = "AbilityItem", menuName = "Scriptable Objects/AbilityItem")]
public class AbilityItem : BaseItem
{
    public GameObject effectPrefab;
    public Sprite abilityIcon;
    public Ability ability;
    public Level level;
    public float DamageMultiplier;
    public string Description;
}
