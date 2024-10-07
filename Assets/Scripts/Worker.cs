using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Stores each job type a worker can be assigned to
/// </summary>
public enum Job
{
    Unassigned,
    Gatherer,
    Lumberjack,
    Miner,
    Builder
}

public class Worker : Creature
{
    [Header("Worker Values")]
    public float gatherRate;
    public float workRange;
    public bool working;
    public Job job;
    public int carryCapacity;
    public int carryAmount;
    public Resource targetResource;
    public bool isOccupyingPosition;
    public Vector3 occupiedPosition;
    public bool hasTargetPosition;
    public Vector3 targetPosition;
    public Resource previouslyTargetedResource;

    [Header("Worker References")]
    public GameObject tool;

    private void Awake()
    {
        // Set references
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Initialise values
        working = false;
        job = Job.Unassigned;
        carryAmount = 0;
        targetResource = null;
        isOccupyingPosition = false;
        hasTargetPosition = false;
    }

    public override void CheckAction()
    {
        // If unit is selected
        if (UnitSelection.instance.selectedUnits.Contains(this))
        {
            // If the action key has been pressed
            if (Input.GetKeyDown(Keybinds.instance.actionKey))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Worker creatures have multiple actions
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, GlobalReferences.instance.workerActionMask))
                {
                    // Clicked on a resource, start collecting it
                    if ((GlobalReferences.instance.resourceMask.value & (1 << hit.transform.gameObject.layer)) != 0)
                    {
                        StopCoroutine(GatherResource());
                        if (isOccupyingPosition)
                        {
                            FormationHandler.instance.FreePosition(targetResource, occupiedPosition);
                            isOccupyingPosition = false;
                        }
                        if (hasTargetPosition)
                        {
                            hasTargetPosition = false;
                            FormationHandler.instance.FreePosition(previouslyTargetedResource, targetPosition);
                        }

                        targetResource = hit.transform.GetComponent<Resource>();
                        StartCoroutine(GatherResource());

                        Vector3 pos = targetResource.transform.position;
                        pos.y += targetResource.transform.lossyScale.y / targetResource.transform.localScale.y + 1;
                        UnitSelection.instance.ShowGatherIndicator(pos);
                    }
                    // Clicked on the ground, move to that location
                    else if ((GlobalReferences.instance.groundMask.value & (1 << hit.transform.gameObject.layer)) != 0)
                    {
                        // Free the previously worked position if there was one
                        if (isOccupyingPosition)
                        {
                            Debug.Log(targetResource + ", " + occupiedPosition);
                            FormationHandler.instance.FreePosition(targetResource, occupiedPosition);
                            isOccupyingPosition = false;
                        }
                        if (hasTargetPosition)
                        {
                            hasTargetPosition = false;
                            FormationHandler.instance.FreePosition(previouslyTargetedResource, targetPosition);
                        }

                        // Disable the tool's animation, if exists
                        if (tool != null)
                        {
                            tool.GetComponent<Animator>().SetBool("working", false);
                        }
                        StopCoroutine(GatherResource());
                        MoveTo(hit.point);
                        working = false;
                        UnitSelection.instance.ShowMoveLocationIndicator(hit.point);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gathers the provided resource over time
    /// </summary>
    public IEnumerator GatherResource()
    {
        // Assign job based on resource type
        ChooseJob(targetResource);

        // Equip correct tool for job
        EquipTool();

        // Move to the resource, if it has spaces
        Vector3 destination = FormationHandler.instance.GetDestination(targetResource.transform.GetComponent<RtsObject>());
        if (destination != Vector3.zero)
        {
            MoveTo(destination);
        }
        else yield break;
        targetPosition = destination; // Store it in case the worker changes destination before reaching this one
        previouslyTargetedResource = targetResource;
        Resource lastRes = targetResource; // Extra variable to check whether resource before wait is same as resource after wait
        hasTargetPosition = true;

        // Wait until the worker has reached its desination
        Vector3 ignoreY = destination;
        ignoreY.y = transform.position.y;

        yield return new WaitUntil(() => Vector3.Distance(ignoreY, transform.position) <= 0.05);
        if (targetResource != lastRes) yield break; // Has changed resource, break this coroutine
        navMeshAgent.destination = transform.position;

        occupiedPosition = destination;
        isOccupyingPosition = true;

        // Enable the tool's animation
        tool.GetComponent<Animator>().SetBool("working", true);

        // Make worker look at the resource
        transform.LookAt(targetResource.transform.position);

        yield break;
    }

    /// <summary>
    /// Gathers from the provided resource's deposit into the worker's carried inventory
    /// </summary>
    public void Gather()
    {
        // If worker's carry capacity is not full
        if (carryAmount < carryCapacity)
        {
            // If resource still has resources left
            if (targetResource.resourceAmount > 0)
            {
                targetResource.resourceAmount--;
                carryAmount++;

                // Check again now that it has less
                if (targetResource.resourceAmount == 0)
                {
                    // The resource is empty, destroy it
                    Destroy(targetResource.gameObject);
                }
            }
            else
            {
                // Find a different resource of same type
                throw new NotImplementedException();
            }

            // Check capacity again now that more has been gathered
            if (carryAmount >= carryCapacity)
            {
                // Set to capacity in case of overflow
                carryAmount = carryCapacity;

                // Disable the tool's animation
                tool.GetComponent<Animator>().SetBool("working", false);
                // Deposit the resources
                StartCoroutine(Deposit());
            }
        }
        else
        {
            // Disable the tool's animation
            tool.GetComponent<Animator>().SetBool("working", false);
            // Deposit the resources
            StartCoroutine(Deposit());
        }
    }

    /// <summary>
    /// Sends the worker to deposit to the home
    /// </summary>
    public IEnumerator Deposit()
    {
        // Free the previously worked position
        FormationHandler.instance.FreePosition(targetResource, occupiedPosition);
        isOccupyingPosition = false;
        hasTargetPosition = false;

        // Move to home
        MoveTo(GlobalReferences.instance.home.transform.position);

        // Wait until the worker is within range of home to deposit resource
        yield return new WaitUntil(() => Vector3.Distance(GlobalReferences.instance.home.transform.position, transform.position) <= workRange + (GlobalReferences.instance.home.transform.lossyScale.x / 2));

        // Deposit the resources into inventory
        Inventory.instance.AddResource(targetResource.resourceType, carryAmount);
        // Remove the resources from worker inventory
        carryAmount = 0;

        // Go back to work
        StartCoroutine(GatherResource());

        yield break;
    }

    /// <summary>
    /// Chooses the correct job based on the provided resource
    /// </summary>
    /// <param name="resource">The resource to work</param>
    private void ChooseJob(Resource resource)
    {
        if (resource == null)
        {
            job = Job.Unassigned;
            carryAmount = 0;
        }
        else if (resource.resourceType == ResourceType.Food)
        {
            if (job != Job.Gatherer)
            {
                job = Job.Gatherer;
                carryAmount = 0;
            }
        }
        else if (resource.resourceType == ResourceType.Wood)
        {
            if (job != Job.Lumberjack)
            {
                job = Job.Lumberjack;
                carryAmount = 0;
            }
        }
        else if (resource.resourceType == ResourceType.Stone || resource.resourceType == ResourceType.Ore)
        {
            if (job != Job.Miner)
            {
                job = Job.Miner;
                carryAmount = 0;
            }
        }
    }

    /// <summary>
    /// Equips the worker with the correct tool for their job
    /// </summary>
    private void EquipTool()
    {
        if (tool != null)
        {
            Destroy(tool);
        }

        if (job == Job.Gatherer)
        {
            tool = Instantiate(GlobalReferences.instance.hoe, transform);
        }
        else if (job == Job.Lumberjack)
        {
            tool = Instantiate(GlobalReferences.instance.axe, transform);
        }
        else if (job == Job.Miner)
        {
            tool = Instantiate(GlobalReferences.instance.pickaxe, transform);
        }

        // Set worker to this worker
        tool.GetComponent<Tool>().worker = this;
    }

    /// <summary>
    /// Sets the worker's job to be the provided job
    /// </summary>
    /// <param name="job">The job to set to</param>
    public void SetJob(Job job)
    {
        this.job = job;
    }

    public override void MoveTo(Vector3 location)
    {
        navMeshAgent.SetDestination(location);
    }
}
