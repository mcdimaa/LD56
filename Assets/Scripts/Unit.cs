using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : RtsObject
{
    [Header("Unit Stats")]
    public int maxHealth;
    public int health;

    private void Awake()
    {
        InitialiseStats();
    }

    /// <summary>
    /// Initialises the units stats
    /// </summary>
    private void InitialiseStats()
    {
        health = maxHealth;
    }
}
