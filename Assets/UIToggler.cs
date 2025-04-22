using UnityEngine;

public class UIToggler : MonoBehaviour
{
    [SerializeField] StatPanelUI statPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            statPanel.togglePanel();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Inventory.Singleton.toggle();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PassiveTreeManager.instance.toggle();
        }
    }
}
