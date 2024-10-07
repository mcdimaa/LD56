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
}
