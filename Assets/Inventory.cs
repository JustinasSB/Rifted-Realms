using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour, IUIToggleable
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    [SerializeField] public InventorySlot[] inventorySlots;
    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] ItemData[] items;
    [SerializeField] AbilityItem[] abilityitems;
    [SerializeField] Button giveItemBtn;
    [SerializeField] Button giveAbilityBtn;
    private Vector3 carriedItemOffset;
    private List<InventorySlot> highlightedSlots = new();
    private EquipmentSlot highlightedSlot;
    private bool clearHightlights = false;
    private MonoBehaviour currentHoveredSlot;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    private bool displaying = false;
    private Vector2 ShowingPosition = new Vector3(1545, 540, 0);
    private Vector2 HiddenPosition = new Vector3(9999, 540, 0);
    public event Action<InventoryItem> OnCarriedItemChange;

    private void Start()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
        giveAbilityBtn.onClick.AddListener(delegate { SpawnAbility(); });
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

        carriedItem.transform.position = Input.mousePosition; //+ carriedItemOffset;

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
    public void Toggle()
    {
        if (!displaying) this.transform.position = ShowingPosition;
        else this.transform.position = HiddenPosition;
        displaying = !displaying;
    }
    public bool IsOpen => displaying;
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
    public void SpawnAbility(AbilityItem item = null)
    {
        AbilityItem _item = item;
        if (_item == null)
        { _item = PickRandomAbiltiy(); }

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
        RectTransform rt = item.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.15f, 0.85f);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.localScale = Vector3.one;
        clearHightlights = withClear;
        this.OnCarriedItemChange?.Invoke(item);
    }
    public void RemoveCarriedItem()
    {
        Inventory.carriedItem = null;
        this.OnCarriedItemChange?.Invoke(null);
    }

    ItemData PickRandomItem()
    {
        int random = UnityEngine.Random.Range(0, items.Length);
        return items[random];
    }
    AbilityItem PickRandomAbiltiy()
    {
        int random = UnityEngine.Random.Range(0, abilityitems.Length);
        return abilityitems[random];
    }
    private (bool, int) TryPlaceItem(ItemData item)
    {
        for (int i = 0; i < inventorySlots.Count(); i++)
        {
            if (inventorySlots[i].CanAllocate(item)) return (true, i);
        }
        return (false, -1);
    }
    private (bool, int) TryPlaceItem(AbilityItem item)
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