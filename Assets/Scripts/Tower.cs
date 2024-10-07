using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    private void Update()
    {
        if (health <= 0)
        {
            GameStateHandler.instance.PlayerVictory();
        }
    }
}
