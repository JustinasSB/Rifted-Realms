using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventorySlot slot;
    public InventoryItem item;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot != null && slot.Allocated)
        {
            RectTransform itemRectTransform = slot.Item.GetComponent<RectTransform>();
            Vector3 itemWorldPosition = itemRectTransform.position;
            Vector3 tooltipPosition = new Vector3(itemWorldPosition.x, itemWorldPosition.y + (slot.Item.data.Size.y / 2) * (slot.Item.data.SlotSize.y / 2) + 10f, itemWorldPosition.z);
            TooltipManager.Show(slot.Item, tooltipPosition);
        }
        if (item != null)
        {
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            Vector3 itemWorldPosition = itemRectTransform.position;
            Vector3 tooltipPosition = new Vector3(itemWorldPosition.x, itemWorldPosition.y + (item.data.Size.y / 2) * (item.data.SlotSize.y / 2) + 10f, itemWorldPosition.z);
            TooltipManager.Show(item, tooltipPosition);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }
    public void OnPointerEnter()
    {
        if (slot != null && slot.Allocated)
        {
            RectTransform itemRectTransform = slot.Item.GetComponent<RectTransform>();
            Vector3 itemWorldPosition = itemRectTransform.position;
            Vector3 tooltipPosition = new Vector3(itemWorldPosition.x, itemWorldPosition.y + (slot.Item.data.Size.y / 2) * (slot.Item.data.SlotSize.y / 2) + 10f, itemWorldPosition.z);
            TooltipManager.Show(slot.Item, tooltipPosition);
        }
        if (item != null)
        {
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            Vector3 itemWorldPosition = itemRectTransform.position;
            Vector3 tooltipPosition = new Vector3(itemWorldPosition.x, itemWorldPosition.y + (item.data.Size.y / 2) * (item.data.SlotSize.y / 2) + 10f, itemWorldPosition.z);
            TooltipManager.Show(item, tooltipPosition);
        }
    }
    public void OnPointerExit()
    {
        TooltipManager.Hide();
    }
}
