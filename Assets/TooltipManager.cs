using System.Collections;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instance;
    public Tooltip tooltip;
    private InventoryItem recent;
    private static readonly Vector2 HiddenPosition = new Vector2(-9999, -9999);
    private Coroutine hideCoroutine;
    private static float hideDelay = 0.05f;
    public void Awake()
    {
        instance = this;
    }
    public static void Show(InventoryItem item, Vector2 position)
    {
        if (instance.recent != null && instance.recent.Equals(item)) 
        {
            if (instance.hideCoroutine != null)
            {
                instance.StopCoroutine(instance.hideCoroutine);
                instance.hideCoroutine = null;
            }
            instance.tooltip.ShowTooltip(); 
        }
        else
        {
            if (instance.hideCoroutine != null)
            {
                instance.StopCoroutine(instance.hideCoroutine);
                instance.hideCoroutine = null;
            }
            instance.recent = item;
            instance.tooltip.ShowTooltip(item, position);
        }
    }
    public static void Hide()
    {
        instance.hideCoroutine = instance.StartCoroutine(instance.DelayedHide());
    }
    private IEnumerator DelayedHide()
    {
        yield return new WaitForSeconds(hideDelay);
        instance.tooltip.panel.transform.position = HiddenPosition;
        instance.hideCoroutine = null;
    }
}
