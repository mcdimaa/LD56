using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    [Header("Singleton")]
    public static GlobalReferences instance;

    [Header("Prefabs - Indicators")]
    public GameObject selectedIndicator;
    public GameObject moveLocationIndicator;
    public GameObject gatherIndicator;

    [Header("Prefabs - Creatures")]
    public GameObject workerCreature;

    [Header("Prefabs - Tools")]
    public GameObject hoe;
    public GameObject axe;
    public GameObject pickaxe;

    [Header("LayerMasks")]
    public LayerMask groundMask;
    public LayerMask resourceMask;
    public LayerMask workerActionMask;
    public LayerMask selectableMask;

    [Header("Other")]
    public GameObject home;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;
    }
}
