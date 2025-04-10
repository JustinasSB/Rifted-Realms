using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tooltip : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text coreValuesText;
    public TMP_Text rarityText;
    public TMP_Text modifiersText;
    public LayoutElement layoutElement;
    public GameObject panel;
    public void ShowTooltip(InventoryItem item, Vector2 position)
    {
        nameText.text = item.data.name;
        rarityText.text = item.Rarity.ToString();
        rarityText.color = ToolTipColor(item.Rarity);
        if (item.Modifiers != null)
        {
            modifiersText.text = string.Join("\n", item.Modifiers.Select(mod => mod.Text.ToString()));
        }
        if (item.Stats != null)
        {
            coreValuesText.text = string.Join("\n",
                item.Stats.List.Select(mod => $"{mod.Key}: {mod.Value.Value}"));
        }
        layoutElement.enabled = (modifiersText.preferredWidth > 800 || nameText.preferredWidth > 800) ? true : false;

        SetPivot(position);

        panel.transform.position = position+Vector2.up*50;
    }
    private void SetPivot(Vector2 position)
    {
        RectTransform rt = GetComponent<RectTransform>();
        float tooltipWidth = rt.rect.width > layoutElement.preferredWidth ? layoutElement.preferredWidth : rt.rect.width;
        float tooltipHeight = rt.rect.height;
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

    public void HideTooltip()
    {
        panel.SetActive(false);
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
}
