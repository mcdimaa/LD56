using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ActionSystem/ActionData")]
public class ActionData : ScriptableObject
{
    [Header("Action References")]
    public string actionName;
    public Sprite icon;
    public float cooldown;
    public bool readyToUse;
    [SerializeReference] public IActionBehaviour actionBehaviour;

    [Header("Action Costs")]
    public int foodCost;
    public int woodCost;
    public int stoneCost;
    public int oreCost;

    public void SetActionBehaviour(IActionBehaviour actionBehaviour)
    {
        this.actionBehaviour = actionBehaviour;
    }

    public void Execute()
    {
        if (ActionManager.instance.CanAffordAction(this))
        {
            if (readyToUse)
            {
                actionBehaviour?.Execute();
                readyToUse = false;
                ActionManager.instance.ReadyActionAfter(this, cooldown);
            }
            else
            {
                GuiHandler.instance.ShowNotification("Action is not ready yet");
                Debug.Log("Action is not ready yet");
            }
        }
        else
        {
            GuiHandler.instance.ShowNotification("Cant afford action");
            Debug.Log("Cant afford action");
        }
    }
}
