using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores each type of resource that exists
/// </summary>
public enum ResourceType
{
    Food,
    Wood,
    Stone,
    Ore
}

public class Inventory : MonoBehaviour
{
    [Header("Singleton")]
    public static Inventory instance;

    [Header("Resources")]
    public int food;
    public int wood;
    public int stone;
    public int ore;


    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;

        ClearInventory();
    }

    /// <summary>
    /// Clears the inventory to empty
    /// </summary>
    private void ClearInventory()
    {
        food = 0;
        wood = 0;
        stone = 0;
        ore = 0;
    }

    /// <summary>
    /// Adds a specified amount of the specified resource to the inventory
    /// </summary>
    /// <param name="resource">The resource to add</param>
    /// <param name="amount">The amount to add</param>
    public void AddResource(ResourceType resource, int amount)
    {
        switch (resource)
        {
            case ResourceType.Food: food += amount; break;
            case ResourceType.Wood: wood += amount; break;
            case ResourceType.Stone: stone += amount; break;
            case ResourceType.Ore: ore += amount; break;

            default: Debug.Log("INVALID RESOURCE TYPE"); break;
        }
    }

    /// <summary>
    /// Removes a specified amount of the specified resource from the inventory
    /// </summary>
    /// <param name="resource">The resource to remove</param>
    /// <param name="amount">The amount to remove</param>
    public void RemoveResource(ResourceType resource, int amount)
    {
        switch (resource)
        {
            case ResourceType.Food: food -= amount; break;
            case ResourceType.Wood: wood -= amount; break;
            case ResourceType.Stone: stone -= amount; break;
            case ResourceType.Ore: ore -= amount; break;

            default: Debug.Log("INVALID RESOURCE TYPE"); break;
        }
    }
}
