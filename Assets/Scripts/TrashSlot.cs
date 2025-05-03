using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Image Border;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (Inventory.carriedItem == null)
        {
            return;
        }
        else if (Inventory.carriedItem != null)
        {
            InventoryItem item = Inventory.carriedItem;
            Inventory.Singleton.RemoveCarriedItem();
            Destroy(item.gameObject);
        }
    }
    private void Start()
    {
        Inventory.Singleton.OnCarriedItemChange += item => CarriedItemChanged(item);
    }
    private void CarriedItemChanged(InventoryItem item)
    {
        if (item == null)
        {
            Border.color = Color.white;
            return;
        }
        Border.color = Color.red;
    }
    public TrashSlot GetTargetSlot()
    {
        return this;
    }
    public void Highlight()
    {
        image.color = new Color32(255,0,0,155);
    }

    public void ResetHighlight()
    {
        image.color = Color.clear;
    }
}
