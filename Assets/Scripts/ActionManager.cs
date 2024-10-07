using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Singleton")]
    public static ActionManager instance;

    [Header("ActionData References")]
    public ActionData createWorkerActionData;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        // Make each action ready
        createWorkerActionData.readyToUse = true;

        // Assign behaviours to each action
        createWorkerActionData.SetActionBehaviour(new CreateWorkerActionBehaviour());
    }

    /// <summary>
    /// Spawns a worker creature at the main camp spawnPoint, also adds it to the RtsObjects list in UnitSelection
    /// </summary>
    public void SpawnWorker()
    {
        GameObject worker = Instantiate(GlobalReferences.instance.workerCreature, GlobalReferences.instance.mainCamp.spawnPoint.position, new Quaternion(0, 0, 0, 0));
        UnitSelection.instance.AddUnit(worker.GetComponent<Worker>());
    }

    public void ReadyActionAfter(ActionData action, float time)
    {
        StartCoroutine(ReadyAction(action, time));
    }

    public IEnumerator ReadyAction(ActionData action, float time)
    {
        yield return new WaitForSeconds(time);

        action.readyToUse = true;

        yield return null;
    }
}
