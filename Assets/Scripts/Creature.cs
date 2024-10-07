using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Creature : Unit
{
    [Header("Creature Values")]
    public float moveSpeed;

    [Header("Creature References")]
    public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        // Set references
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckAction();
    }

    /// <summary>
    /// Checks whether the creature is ordered to perform an action
    /// </summary>
    public virtual void CheckAction()
    {
        // If unit is selected
        if (UnitSelection.instance.selectedObjects.Contains(this))
        {
            // If the action key has been pressed
            if (Input.GetKeyDown(Keybinds.instance.actionKey))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Base creature only has move action
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, GlobalReferences.instance.groundMask))
                {
                    MoveTo(hit.point);
                    UnitSelection.instance.ShowMoveLocationIndicator(hit.point);
                }
            }
        }
    }

    /// <summary>
    /// Moves the creature to the specified location
    /// </summary>
    /// <param name="location">Where to move to</param>
    public virtual void MoveTo(Vector3 location)
    {
        navMeshAgent.SetDestination(location);
    }

    public override List<Tuple<string, string>> GetObjectInfo()
    {
        List<Tuple<string, string>> infoList = new List<Tuple<string, string>>();

        infoList.Add(new Tuple<string, string>("Name", rtsName));
        infoList.Add(new Tuple<string, string>("Health", health.ToString() + "/" + maxHealth.ToString()));
        infoList.Add(new Tuple<string, string>("MoveSpeed", moveSpeed.ToString()));

        return infoList;
    }
}
