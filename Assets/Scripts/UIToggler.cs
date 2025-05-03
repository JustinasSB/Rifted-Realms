using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml.Linq;

public class UIToggler : MonoBehaviour
{
    [SerializeField] StatPanelUI statPanel;
    [SerializeField] GameOver EndGamePanel;
    Stack<IUIToggleable> activePanels = new(); 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePanels.Count > 0)
            {
                IUIToggleable top = activePanels.Pop();
                top.Toggle();
            }
            else
            {
                HandleToggle(EndGamePanel);
            }
        }
        if (EndGamePanel.isOpen && DeathManager.Dead == false) return;
        if (Input.GetKeyDown(KeyCode.C))
        {
            HandleToggle(statPanel);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HandleToggle(Inventory.Singleton);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            HandleToggle(PassiveTreeManager.instance);
        }
    }
    private void HandleToggle(IUIToggleable panel)
    {
        panel.Toggle();

        if (panel.IsOpen)
        {
            activePanels.Push(panel);
        }
        else
        {
            RemoveFromStack(panel);
        }
    }
    private void RemoveFromStack(IUIToggleable panel)
    {
        Stack<IUIToggleable> temp = new();
        while (activePanels.Count > 0)
        {
            var item = activePanels.Pop();
            if (item != panel) temp.Push(item);
        }
        activePanels = temp;
    }
}
public interface IUIToggleable
{
    void Toggle();
    bool IsOpen { get; }
}
