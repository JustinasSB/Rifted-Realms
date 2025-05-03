using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text nameText;
    public TMP_Text coreValuesText;
    public TMP_Text rarityText;
    public TMP_Text modifiersText;
    public LayoutElement layoutElement;
    public GameObject panel;
    public RectTransform[] backgrounds;
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Image border;
    private Vector2 lastpos;
    private bool setToWorld = false;
    private Material Regular;
    private Material World;

    public void ShowTooltip(InventoryItem item, Vector2 position)
    {
        StartCoroutine(DelayedReposition());
        lastpos = position;
        if (item.ItemName != null)
        {
            itemNameText.text = item.ItemName;
        }
        if (item.item != null)
        {
            itemTooltip(item, position);
            return;
        }
        else if (item.ability != null)
        {
            abilityTooltip(item, position);
            return;
        }
    }
    public void ShowTooltip(PassiveTooltipData nodeData, Vector2 position)
    {
        StartCoroutine(DelayedReposition());
        lastpos = position;

        itemNameText.text = "";
        nameText.text = nodeData.Name;
        coreValuesText.text = "";
        rarityText.text = "Passive Node";
        rarityText.color = Color.white;
        background.color = Color.black;
        border.color = Color.white;
        modifiersText.text = nodeData.Description;

        layoutElement.enabled = (modifiersText.preferredWidth > 800 || nameText.preferredWidth > 800);
    }
    private void abilityTooltip(InventoryItem item, Vector2 position)
    {
        nameText.text = item.ability.ItemName.Replace("_", " ");
        rarityText.color = Color.green;
        background.color = Color.green;
        border.color = Color.green;
        if (setToWorld)
        {
            rarityText.fontMaterial = Regular;
            setToWorld = false;
        }
        modifiersText.text = item.ability.Description;
        if (item.ability.ability.support)
        {
            rarityText.text = "Support Jewel";
            coreValuesText.text = "Mana cost multiplier: " + item.ability.Multiplier + "%";
        }
        else
        {
            rarityText.text = "Ability Jewel";
            coreValuesText.text = "Damage multiplier: " + item.ability.Multiplier + "%\n"
                + "Mana Cost: " + item.ability.ManaCost;

        }
        layoutElement.enabled = (modifiersText.preferredWidth > 800 || nameText.preferredWidth > 800) ? true : false;
    }
    private void itemTooltip(InventoryItem item, Vector2 position)
    {
        nameText.text = item.data.name.Replace("_", " "); ;
        if (item.Rarity != Rarity.None)
        {
            rarityText.text = item.Rarity.ToString();
            rarityText.color = ToolTipColor(item.Rarity);
            background.color = ToolTipColor(item.Rarity);
            border.color = ToolTipColor(item.Rarity);
            if (item.Rarity == Rarity.World && !setToWorld)
            {
                rarityText.fontMaterial = World;
                setToWorld = true;
            }
            else if (item.Rarity != Rarity.World && setToWorld)
            {
                rarityText.fontMaterial = Regular;
                setToWorld = false;
            }
        }
        else
        {
            rarityText.text = "";
            background.color = ToolTipColor(Rarity.Common);
            border.color = ToolTipColor(Rarity.Common);
        }
        if (item.Modifiers != null)
        {
            List<ItemModifier> displayModifiers = new List<ItemModifier>();
            foreach (ItemModifier mod in item.Modifiers.AsEnumerable().Reverse())
            {
                var found = displayModifiers.FirstOrDefault(m =>
                    m.AffectedStat == mod.AffectedStat &&
                    m.OperationType == mod.OperationType);
                if (found != null)
                {
                    found.AddToRolledValue(mod.RolledValue);
                }
                else
                {
                    displayModifiers.Add(new ItemModifier(mod));
                }
            }

            modifiersText.text = string.Join("\n", displayModifiers
                .OrderBy(mod => mod.Type)
                .ThenByDescending(mod => mod.OperationType)
                .ThenByDescending(mod => mod.Text)
                .Select(mod => mod.Text));
        }
        else modifiersText.text = "";
        if (item.Stats != null)
        {
            coreValuesText.text = string.Join("\n",
                item.Stats.List
                .OrderByDescending(mod => mod.Key.GetDisplayName())
                .Select(mod => $"{mod.Key.GetDisplayName()}: {mod.Value.Value}"));
        }
        else coreValuesText.text = "";
        layoutElement.enabled = (modifiersText.preferredWidth > 800 || nameText.preferredWidth > 800) ? true : false;
    }
    private void SetPivot(Vector2 position, float height)
    {
        RectTransform rt = GetComponent<RectTransform>();
        float tooltipWidth = rt.rect.width > layoutElement.preferredWidth ? layoutElement.preferredWidth : backgrounds[0].sizeDelta.x;
        float tooltipHeight = height;
        float pivotX = 0.5f;
        float pivotY = 0.5f;
        if (position.x + tooltipWidth / 2 > Screen.width || position.x - tooltipWidth / 2 < 0)
        {
            pivotX = position.x / Screen.width;
        }
        if (position.y + tooltipHeight / 2 > Screen.height || position.y - tooltipHeight / 2 < 0)
        {
            pivotY = position.y / Screen.height;
        }
        rt.pivot = new Vector2(pivotX, pivotY);
    }
    private IEnumerator DelayedReposition()
    {
        yield return new WaitForSeconds(0.01f);
        float height = 0;
        foreach (RectTransform bg in backgrounds)
        {
            height += bg.sizeDelta.y;
        }
        Vector2 SetPos = new Vector2(lastpos.x, lastpos.y + height / 2);
        SetPivot(SetPos, height);
        panel.transform.position = SetPos;
    }

    private Color ToolTipColor(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => Color.gray,
            Rarity.Magic => Color.cyan,
            Rarity.Rare => Color.yellow,
            Rarity.Legendary => new Color(1f, 0.5f, 0f),
            Rarity.Divine => Color.magenta,
            Rarity.World => new Color(0.15f, 0.15f, 0.15f),
            _ => Color.white
        };
    }
    public void Start()
    {
        World = new Material(rarityText.fontSharedMaterial);
        World.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.25f);
        World.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
        Regular = new Material(rarityText.fontSharedMaterial);
        Regular.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        Regular.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
    }
}
