using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RtsObject : MonoBehaviour
{
    [Header("Object Values")]
    public string rtsName;
    public List<ActionData> actions;

    /// <summary>
    /// Shows the SelectionIndicator under the object
    /// </summary>
    public virtual void Select()
    {
        GameObject selectionIndicator = Instantiate(GlobalReferences.instance.selectedIndicator, transform);
        Vector3 newPos = selectionIndicator.transform.position;
        newPos.y = 0;
        selectionIndicator.transform.position = newPos;
        selectionIndicator.name = "SelectionIndicator";
    }

    /// <summary>
    /// Hides the SelectionIndicator under the object
    /// </summary>
    public virtual void Deselect()
    {
        Destroy(transform.Find("SelectionIndicator").gameObject);
    }

    /// <summary>
    /// Gets all the object information and returns it as a list with info names and values, both strings
    /// </summary>
    /// <returns>A List of double string Tuples containting the info names and values</returns>
    public virtual List<Tuple<string, string>> GetObjectInfo()
    {
        List<Tuple<string, string>> infoList = new List<Tuple<string, string>>();

        infoList.Add(new Tuple<string, string>("Name", rtsName));

        return infoList;
    }

    public virtual List<ActionData> GetObjectActions()
    {
        return actions;
    }

    public void OnDestroy()
    {
        UnitSelection.instance.rtsObjects.Remove(this);
    }
}
