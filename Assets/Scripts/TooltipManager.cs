using System.Collections;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instance;
    public Tooltip tooltip;
    private InventoryItem recent;
    private static readonly Vector2 HiddenPosition = new Vector2(-9999, -9999);
    private Coroutine hideCoroutine;
    private static float hideDelay = 0.02f;
    public static bool displaying = false;
    private PassiveTooltipData recentPassive;
    public void Awake()
    {
        instance = this;
    }
    public static void Show(InventoryItem item, Vector2 position)
    {
        if (instance.recent != null && item != null && instance.recent.Equals(item))
        {
            if (instance.hideCoroutine != null)
            {
                instance.StopCoroutine(instance.hideCoroutine);
                instance.hideCoroutine = null;
            }
            displaying = true;
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
            displaying = true;
        }
    }
    public static void Show(PassiveTooltipData nodeData, Vector2 position)
    {
        if (instance.recentPassive != null && nodeData != null &&
            instance.recentPassive.Id == nodeData.Id)
        {
            if (instance.hideCoroutine != null)
            {
                instance.StopCoroutine(instance.hideCoroutine);
                instance.hideCoroutine = null;
            }
            displaying = true;
        }
        else
        {
            if (instance.hideCoroutine != null)
            {
                instance.StopCoroutine(instance.hideCoroutine);
                instance.hideCoroutine = null;
            }
            instance.recentPassive = nodeData;
            instance.tooltip.ShowTooltip(nodeData, position);
            displaying = true;
        }
    }
    public static void Hide()
    {
        if (instance.hideCoroutine != null) return;
        instance.hideCoroutine = instance.StartCoroutine(instance.DelayedHide());
    }
    public static void DestroyMemory()
    {
        instance.recent = null;
        instance.recentPassive = null;
    }
    private IEnumerator DelayedHide()
    {
        yield return new WaitForSeconds(hideDelay);
        instance.tooltip.panel.transform.position = HiddenPosition;
        instance.hideCoroutine = null;
        DestroyMemory();
        displaying = false;
    }
    public static bool DisplayingThis(InventoryItem toCheck)
    {
        if (!displaying) return false;
        if (instance.recent != null && toCheck != null && instance.recent.Equals(toCheck))
        {
            return true;
        }
        return false;
    }

}
