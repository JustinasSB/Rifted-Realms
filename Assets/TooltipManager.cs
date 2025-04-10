using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instance;
    public Tooltip tooltip;
    public void Awake()
    {
        instance = this;
    }
    //public static void Show()
    //{
    //    instance.tooltip.gameObject.SetActive(true);
    //}
    public static void Show(InventoryItem item, Vector2 position)
    {
        instance.tooltip.ShowTooltip(item, position);
        instance.tooltip.panel.SetActive(true);
    }
    public static void Hide()
    {
        instance.tooltip.gameObject.SetActive(false);
    }
}
