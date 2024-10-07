using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : Building
{
    [Header("Camp References")]
    public Transform spawnPoint;

    public void OnDestroy()
    {
        GameStateHandler.instance.PlayerDefeat();
    }
}
