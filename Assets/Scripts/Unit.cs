using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : RtsObject
{
    [Header("Unit Stats")]
    public int maxHealth;
    public int health;
    public float moveSpeed;

    [Header("Unit References")]
    public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        // Set references
        navMeshAgent = GetComponent<NavMeshAgent>();
        InitialiseStats();
    }

    /// <summary>
    /// Initialises the units stats
    /// </summary>
    private void InitialiseStats()
    {
        health = maxHealth;
    }

    public override List<Tuple<string, string>> GetObjectInfo()
    {
        List<Tuple<string, string>> infoList = new List<Tuple<string, string>>();

        infoList.Add(new Tuple<string, string>("Name", rtsName));
        infoList.Add(new Tuple<string, string>("Health", health.ToString() + "/" + maxHealth.ToString()));

        return infoList;
    }
}
