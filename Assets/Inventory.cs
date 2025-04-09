using System.Collections.Generic;
using System.Linq;
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
    private EquipmentSlot highlightedSlot;
    private bool clearHightlights = false;
    private MonoBehaviour currentHoveredSlot;
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

        RaycastResult? hoveredResult = GetHoveredSlotResult();
        if (!clearHightlights)
        {
            if (hoveredResult == null)
            {
                ClearHighlights();
                currentHoveredSlot = null;
                return;
            }
            if (hoveredResult.Value.gameObject == currentHoveredSlot?.gameObject)
                return;
        }

        clearHightlights = false;
        ClearHighlights();

        if (hoveredResult == null)
        {
            currentHoveredSlot = null;
            return;
        }

        GameObject hoveredObject = hoveredResult.Value.gameObject;
        currentHoveredSlot = hoveredObject.GetComponent<MonoBehaviour>(); // still stored for tracking

        if (hoveredObject.TryGetComponent(out InventorySlot invSlot))
        {
            HighlightInventorySlots(invSlot);
        }
        else if (hoveredObject.TryGetComponent(out EquipmentSlot equipSlot))
        {
            HighlightEquipmentSlots(equipSlot);
        }
    }
    private RaycastResult? GetHoveredSlotResult()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<InventorySlot>() != null ||
                result.gameObject.GetComponent<EquipmentSlot>() != null)
            {
                return result;
            }
        }

        return null;
    }
    private void HighlightInventorySlots(InventorySlot slot)
    {
        int status = slot.CanAllocate(Inventory.carriedItem);
        List<InventorySlot> slotsToHighlight = slot.GetTargetSlots(Inventory.carriedItem);

        foreach (var s in slotsToHighlight)
        {
            s.Highlight(status == 2 ? Color.red.WithAlpha(50f / 255f) : Color.green.WithAlpha(50f / 255f));
            highlightedSlots.Add(s);
        }
    }

    private void HighlightEquipmentSlots(EquipmentSlot slot)
    {
        int status = slot.CanAllocate(Inventory.carriedItem);
        EquipmentSlot slotToHighlight = slot.GetTargetSlot();
        slotToHighlight.Highlight(status == 2 ? Color.red.WithAlpha(50f / 255f) : Color.green.WithAlpha(50f / 255f));
        highlightedSlot = slotToHighlight;
    }

    public void SpawnInventoryItem(ItemData item = null)
    {
        ItemData _item = item;
        if (_item == null)
        { _item = PickRandomItem(); }

        (bool possible, int location) = TryPlaceItem(_item);
        if (possible) 
        {
            Instantiate(itemPrefab).InitializeInInventory(_item, location);
            //Instantiate(itemPrefab, inventorySlots[location].transform).InitializeInInventory(_item, inventorySlots[location]);
        }
    }
    private void ClearHighlights()
    {
        if (highlightedSlot != null)
        {
            highlightedSlot.ResetHighlight();
            highlightedSlot = null;
        }
        foreach (var s in highlightedSlots)
            s.ResetHighlight();
        highlightedSlots.Clear();
    }
    public InventorySlot GetSlot(int index)
    {
        return inventorySlots[index];
    }
    public void SetCarriedItem(InventoryItem item, bool withClear)
    {
        if (carriedItem != null) return;

        carriedItem = item;
        item.transform.SetParent(draggablesTransform);
        if (item.data.SlotSize != Vector2.one)
        {
            carriedItemOffset = new Vector2(item.data.SlotSize.x * item.data.Size.x / 2 - item.data.Size.x / 2 - 10, -item.data.SlotSize.y * item.data.Size.y / 2 + item.data.Size.y / 2 + 10);
        }
        clearHightlights = withClear;
    }

    ItemData PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }
    private (bool, int) TryPlaceItem(ItemData item)
    {
        for (int i = 0; i < inventorySlots.Count(); i++)
        {
            if (inventorySlots[i].CanAllocate(item)) return (true, i);
        }
        return (false, -1);
    }
    public void PlaceItem(InventoryItem item, int location)
    {
        inventorySlots[location].allocateItem(item);
    }
}