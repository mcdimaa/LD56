using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RtsObject : MonoBehaviour
{
    [Header("Object Values")]
    public string rtsName;

    /// <summary>
    /// Shows the SelectionIndicator under the object
    /// </summary>
    public void Select()
    {
        GameObject selectionIndicator = Instantiate(GlobalReferences.instance.selectedIndicator, transform);
        selectionIndicator.name = "SelectionIndicator";
    }

    /// <summary>
    /// Hides the SelectionIndicator under the object
    /// </summary>
    public void Deselect()
    {
        Destroy(transform.Find("SelectionIndicator").gameObject);
    }
}
