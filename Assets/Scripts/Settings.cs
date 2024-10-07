using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Singleton")]
    public static Settings instance;

    [Header("Settings")]
    public bool edgePanning;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
