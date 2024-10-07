using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuiHandler : MonoBehaviour
{
    [Header("Singleton")]
    public static GuiHandler instance;

    [Header("UI References")]
    public VisualElement uiDocument;

    public Label foodNumber;
    public Label woodNumber;
    public Label stoneNumber;
    public Label oreNumber;

    public Button addFoodButton;
    public Button addWoodButton;
    public Button addStoneButton;
    public Button addOreButton;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;

        // Set other references
        uiDocument = GetComponent<UIDocument>().rootVisualElement;

        foodNumber = uiDocument.Q<Label>("FoodNumber");
        woodNumber = uiDocument.Q<Label>("WoodNumber");
        stoneNumber = uiDocument.Q<Label>("StoneNumber");
        oreNumber = uiDocument.Q<Label>("OreNumber");

        addFoodButton = uiDocument.Q<Button>("FoodButton");
        addFoodButton.clicked += AddFood;
        addWoodButton = uiDocument.Q<Button>("WoodButton");
        addWoodButton.clicked += AddWood;
        addStoneButton = uiDocument.Q<Button>("StoneButton");
        addStoneButton.clicked += AddStone;
        addOreButton = uiDocument.Q<Button>("OreButton");
        addOreButton.clicked += AddOre;
    }

    private void Update()
    {
        UpdateResourceLabels();
    }

    /// <summary>
    /// Updates the number label of each resource to match the inventory amounts
    /// </summary>
    private void UpdateResourceLabels()
    {
        foodNumber.text = Inventory.instance.food.ToString();
        woodNumber.text = Inventory.instance.wood.ToString();
        stoneNumber.text = Inventory.instance.stone.ToString();
        oreNumber.text = Inventory.instance.ore.ToString();
    }

    /// <summary>
    /// Adds 100 food to the inventory
    /// </summary>
    private void AddFood()
    {
        Inventory.instance.AddResource(ResourceType.Food, 100);
    }

    /// <summary>
    /// Adds 100 wood to the inventory
    /// </summary>
    private void AddWood()
    {
        Inventory.instance.AddResource(ResourceType.Wood, 100);
    }

    /// <summary>
    /// Adds 100 stone to the inventory
    /// </summary>
    private void AddStone()
    {
        Inventory.instance.AddResource(ResourceType.Stone, 100);
    }

    /// <summary>
    /// Adds 100 ore to the inventory
    /// </summary>
    private void AddOre()
    {
        Inventory.instance.AddResource(ResourceType.Ore, 100);
    }
}
