using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [Header("Tool References")]
    public Worker worker;

    /// <summary>
    /// Call's the worker's Gather function
    /// </summary>
    public void Gather()
    {
        worker.Gather();
    }
}
