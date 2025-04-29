using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class TooltipTriggerPassiveNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PassiveTreeNode node;
    private bool hovering = false;
    private Coroutine showCoroutine;
    private Vector2 lastMousePosition;
    private Vector2 slotPos;
    private const float showDelay = 0.02f;

    private void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        slotPos = new Vector2(rt.position.x, rt.position.y + 25f);
    }

    private void Update()
    {
        if (!hovering) return;
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        TryShow(pos);
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
            if (node != null)
            {
                PassiveTooltipData data = new PassiveTooltipData(node.Id, node.Name, node.Description);
                TooltipManager.Show(data, position);
            }
        }
        showCoroutine = null;
    }
}

