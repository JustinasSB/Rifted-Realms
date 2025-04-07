using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    [SerializeField] public InventorySlot[] inventorySlots;
    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] ItemData[] items;
    [SerializeField] Button giveItemBtn;
    private Vector3 carriedItemOffset;
    private List<InventorySlot> highlightedSlots = new();
    private bool cleared = true;
    private InventorySlot currentHoveredSlot;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    private void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
        if (raycaster == null)
            raycaster = GetComponent<GraphicRaycaster>();
        if (eventSystem == null)
            eventSystem = EventSystem.current;
    }
    private void Update()
    {
        if (carriedItem == null) 
        {
            ClearHighlights();
            return; 
        }
        carriedItem.transform.position = Input.mousePosition + carriedItemOffset;
        InventorySlot slot = GetHoveredSlot();
        if (slot == null || slot == currentHoveredSlot)
            return;

        ClearHighlights();
        currentHoveredSlot = slot;

        int status = slot.CanAllocate(Inventory.carriedItem);
        List<InventorySlot> slotsToHighlight = slot.GetTargetSlots(Inventory.carriedItem);

        foreach (var s in slotsToHighlight)
        {
            s.Highlight(status == 2 ? Color.red.WithAlpha(50f/255f) : Color.green.WithAlpha(50f/255f));
            highlightedSlots.Add(s);
        }
    }
    public InventorySlot GetHoveredSlot()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            InventorySlot slot = result.gameObject.GetComponent<InventorySlot>();
            if (slot != null)
                return slot;
        }

        return null;
    }

    public void SpawnInventoryItem(ItemData item = null)
    {
        ItemData _item = item;
        if (_item == null)
        { _item = PickRandomItem(); }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Check if the slot is empty
            if (inventorySlots[i].Item == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }
    private void ClearHighlights()
    {
        foreach (var s in highlightedSlots)
            s.ResetHighlight();
        highlightedSlots.Clear();
    }
    public InventorySlot GetSlot(int index)
    {
        return inventorySlots[index];
    }
    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null) return;

        carriedItem = item;
        item.transform.SetParent(draggablesTransform);
        if (item.data.SlotSize != Vector2.one)
        {
            carriedItemOffset = new Vector2(item.data.SlotSize.x * item.data.Size.x / 2 - item.data.Size.x / 2 - 10, -item.data.SlotSize.y * item.data.Size.y / 2 + item.data.Size.y / 2 + 10);
        }
        item.itemIcon.raycastTarget = false;
    }

    ItemData PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }
}