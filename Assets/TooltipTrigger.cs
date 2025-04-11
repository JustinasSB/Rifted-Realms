using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;
    public RectTransform rt;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rt != null && item != null)
        {
            Vector3 tooltipPosition = new Vector3(rt.position.x, rt.position.y + 25f, rt.position.z);
            TooltipManager.Show(item, tooltipPosition);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }
    public void OnPointerEnter()
    {
        if (rt != null && item != null)
        {
            Vector3 tooltipPosition = new Vector3(rt.position.x, rt.position.y + 25f, rt.position.z);
            TooltipManager.Show(item, tooltipPosition);
        }
    }
    public void OnPointerExit()
    {
        TooltipManager.Hide();
    }
}
