using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;
    public RectTransform rt;
    bool hovering = false;
    private Coroutine showCoroutine;
    private Vector2 lastMousePosition;
    private Vector2 slotPos;
    private const float showDelay = 0.02f;
    private void Update()
    {
        if (!hovering) return;
        if (TooltipManager.DisplayingThis(item))
        {
            TooltipManager.Show(item, slotPos);
            hovering = false;
            return;
        }
        TryShow(slotPos);
    }
    public void Initialize()
    {
        slotPos = new Vector3(rt.position.x, rt.position.y + 25f, rt.position.z);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
        hovering = false;
    }
    public void OnPointerEnter()
    {
        hovering = true;
    }
    public void ResetManager()
    {
        TooltipManager.DestroyMemory();
    }
    public void OnPointerExit()
    {
        TooltipManager.Hide();
        hovering = false;
    }
    public void TryShow(Vector2 position)
    {
        if (showCoroutine != null)
        {
            return;
        }
        showCoroutine = StartCoroutine(DelayedShow(position));
    }
    private IEnumerator DelayedShow(Vector2 position)
    {
        lastMousePosition = Input.mousePosition;
        yield return new WaitForSeconds(showDelay);

        if (Vector2.Distance(Input.mousePosition, lastMousePosition) < 1f)
        {
            if (rt != null && item != null)
            {
                Vector3 tooltipPosition = new Vector3(rt.position.x, rt.position.y + 25f, rt.position.z);
                TooltipManager.Show(item, tooltipPosition);
            }
        }
        showCoroutine = null;
    }
}
