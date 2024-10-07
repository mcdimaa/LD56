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

    public void SetActionBehaviour(IActionBehaviour actionBehaviour)
    {
        this.actionBehaviour = actionBehaviour;
    }

    public void Execute()
    {
        if (readyToUse)
        {
            actionBehaviour?.Execute();
            readyToUse = false;
            ActionManager.instance.ReadyActionAfter(this, cooldown);
        }
        else
        {
            Debug.Log("Action is not ready yet");
        }
    }
}
