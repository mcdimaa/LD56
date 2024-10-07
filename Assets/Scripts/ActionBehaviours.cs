using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorkerActionBehaviour : IActionBehaviour
{
    public void Execute()
    {
        ActionManager.instance.SpawnWorker();
    }
}

public class CreateFighterActionBehaviour : IActionBehaviour
{
    public void Execute()
    {
        ActionManager.instance.SpawnFighter();
    }
}