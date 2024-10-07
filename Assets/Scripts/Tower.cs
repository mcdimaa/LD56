using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tower : Building
{
    [Header("Tower Values")]
    public Transform spawnPoint;
    public float spawnInterval;

    public void Start()
    {
        InvokeRepeating("SpawnGolem", spawnInterval, spawnInterval);
    }

    public void SpawnGolem()
    {
        int golem = UnityEngine.Random.Range(0, 2);

        if (golem == 0)
        {
            Instantiate(GlobalReferences.instance.fighterGolem, spawnPoint.position, new Quaternion(0, 0, 0, 0));
        }
        else
        {
            Instantiate(GlobalReferences.instance.tankGolem, spawnPoint.position, new Quaternion(0, 0, 0, 0));
        }
    }

    public void OnDestroy()
    {
        GameStateHandler.instance.PlayerVictory();
    }
}
