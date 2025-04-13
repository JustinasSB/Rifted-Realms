using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

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
    private Coroutine Reposition;

    public void ShowTooltip(InventoryItem item, Vector2 position)
    {
        Reposition = StartCoroutine(DelayedReposition());
        lastpos = position;
        if (item.ItemName != null)
        {
            itemNameText.text = item.ItemName;
        }
        nameText.text = item.data.name;
        rarityText.text = item.Rarity.ToString();
        rarityText.color = ToolTipColor(item.Rarity);
        background.color = ToolTipColor(item.Rarity);
        border.color = ToolTipColor(item.Rarity);
        if (item.Rarity == Rarity.World && !setToWorld)
        {
            rarityText.fontMaterial = World;
        }
        else if (setToWorld)
        {
            rarityText.fontMaterial = Regular;
        }
        if (item.Modifiers != null)
        {
            modifiersText.text = string.Join("\n", item.Modifiers.Select(mod => mod.Text.ToString()));
        }
        if (item.Stats != null)
        {
            coreValuesText.text = string.Join("\n",
                item.Stats.List.Select(mod => $"{mod.Key.GetDisplayName()}: {mod.Value.Value}"));
        }
        layoutElement.enabled = (modifiersText.preferredWidth > 800 || nameText.preferredWidth > 800) ? true : false;
        SetPivot(position);
        float height = 0;
        foreach (RectTransform bg in backgrounds)
        {
            height += bg.sizeDelta.y;
        }
        panel.transform.position = new Vector2(position.x, position.y + height / 2);
    }
    public void ShowTooltip()
    {
        Reposition = StartCoroutine(DelayedReposition());
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
    private IEnumerator DelayedReposition()
    {
        yield return new WaitForSeconds(0.01f);
        float height = 0;
        foreach (RectTransform bg in backgrounds)
        {
            height += bg.sizeDelta.y;
        }
        panel.transform.position = new Vector2(lastpos.x, lastpos.y + height / 2);
        Reposition = null;
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
        Material newMat = new Material(rarityText.fontSharedMaterial);
        newMat.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        newMat.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
        World = newMat;
        newMat.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        newMat.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        Regular = newMat;
    }
}
