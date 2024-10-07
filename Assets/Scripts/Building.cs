using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : RtsObject
{
    [Header("Building Values")]
    public int maxHealth;
    public int health;

    public override List<Tuple<string, string>> GetObjectInfo()
    {
        List<Tuple<string, string>> infoList = new List<Tuple<string, string>>();

        infoList.Add(new Tuple<string, string>("Name", rtsName));
        infoList.Add(new Tuple<string, string>("Health", health.ToString() + "/" + maxHealth.ToString()));

        return infoList;
    }

    public void TakeDamage(int amount)
    {
        if (health - amount <= 0)
        {
            Break();
        }
        else
        {
            health -= amount;
        }
    }

    public void Break()
    {
        Destroy(gameObject);
    }
}
