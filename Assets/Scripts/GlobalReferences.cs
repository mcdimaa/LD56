using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    [Header("Singleton")]
    public static GlobalReferences instance;

    [Header("Prefabs - Indicators")]
    public GameObject selectedIndicator;
    public GameObject enemySelectedIndicator;
    public GameObject moveLocationIndicator;
    public GameObject gatherIndicator;
    public GameObject attackIndicator;

    [Header("Prefabs - Creatures")]
    public GameObject workerCreature;
    public GameObject fighterCreature;

    [Header("Prefabs - Golems")]
    public GameObject fighterGolem;
    public GameObject tankGolem;

    [Header("Prefabs - Tools")]
    public GameObject hoe;
    public GameObject axe;
    public GameObject pickaxe;

    [Header("Prefabs - Buildings")]
    public GameObject camp;

    [Header("LayerMasks")]
    public LayerMask groundMask;
    public LayerMask resourceMask;
    public LayerMask unitMask;
    public LayerMask buildingMask;
    public LayerMask workerActionMask;
    public LayerMask fighterActionMask;
    public LayerMask selectableMask;

    [Header("Other")]
    public Camp mainCamp;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;
    }
}
