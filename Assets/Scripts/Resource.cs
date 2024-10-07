using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : RtsObject
{
    [Header("Resource Values")]
    public ResourceType resourceType;
    public int maxResourceAmount;
    public int resourceAmount;
    public float gatherSpeed;

    private void Awake()
    {
        // Initialise values
        resourceAmount = maxResourceAmount;
    }

    public override void Select()
    {
        GameObject selectionIndicator = Instantiate(GlobalReferences.instance.selectedIndicator, transform.Find("IndicatorScaler"));
        Vector3 newPos = selectionIndicator.transform.position;
        newPos.y = 0;
        selectionIndicator.transform.position = newPos;
        selectionIndicator.name = "SelectionIndicator";
    }

    public override void Deselect()
    {
        Destroy(transform.Find("IndicatorScaler").Find("SelectionIndicator").gameObject);
    }

    public override List<Tuple<string, string>> GetObjectInfo()
    {
        List<Tuple<string, string>> infoList = new List<Tuple<string, string>>();

        infoList.Add(new Tuple<string, string>("Name", rtsName));
        infoList.Add(new Tuple<string, string>("Resources", resourceAmount.ToString() + "/" + maxResourceAmount.ToString()));

        return infoList;
    }
}
