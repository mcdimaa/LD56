using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon References")]
    public Unit owner;

    /// <summary>
    /// Call's the owner's Attack function
    /// </summary>
    public void Attack()
    {
        if (owner is Fighter)
        {
            Fighter fighter = (Fighter)owner;
            fighter.Attack();
        }
        else if (owner is Golem)
        {
            Golem golem = (Golem)owner;
            golem.Attack();
        }
    }
}
