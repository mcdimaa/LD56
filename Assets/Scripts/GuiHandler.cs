using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class GuiHandler : MonoBehaviour
{
    [Header("Singleton")]
    public static GuiHandler instance;

    [Header("UI References")]
    public VisualElement uiDocument;
    public GameObject victoryDocument;
    public GameObject defeatDocument;

    public Label foodNumber;
    public Label woodNumber;
    public Label stoneNumber;
    public Label oreNumber;

    public Button addFoodButton;
    public Button addWoodButton;
    public Button addStoneButton;
    public Button addOreButton;

    public VisualElement statsBox;
    public VisualElement actionsBox;

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

        statsBox = uiDocument.Q<VisualElement>("StatsBox");
        actionsBox = uiDocument.Q<VisualElement>("ActionsBox");

        // Disable victory & defeat documents for now
        victoryDocument.SetActive(false);
        defeatDocument.SetActive(false);
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

    /// <summary>
    /// Displays onto the GUI the provided info
    /// </summary>
    /// <param name="infoList">Info to display</param>
    public void DisplayInfo(List<Tuple<string, string>> infoList)
    {
        foreach (Tuple<string, string> tuple in infoList)
        {
            // Initialise VisualElements
            VisualElement statBar = new VisualElement();
            VisualElement statName = new VisualElement();
            VisualElement statValue = new VisualElement();

            // Apply style to statBar
            statBar.AddToClassList("statBar");

            // Initialise Labels & their text
            Label nameLabel = new Label(tuple.Item1);
            Label valueLabel = new Label(tuple.Item2);

            // Apply style to each label
            nameLabel.AddToClassList("paragraphBold");
            valueLabel.AddToClassList("paragraph");

            // Add labels to their parents
            statName.Add(nameLabel);
            statValue.Add(valueLabel);

            // Add stat name and value to the statBar
            statBar.Add(statName);
            statBar.Add(statValue);

            // Add statBar to the statBox
            statsBox.Add(statBar);
        }
    }

    public void ClearDisplayInfo()
    {
        statsBox.Clear();
    }

    public void DisplayActions(List<ActionData> actionList)
    {
        foreach (ActionData action in actionList)
        {
            // Initialise VisualElements
            Button actionButton = new Button();
            Label actionLabel = new Label();
            VisualElement actionIcon = new VisualElement();

            // Disable button if ability is not ready yet
            if (!action.readyToUse)
            {
                actionButton.SetEnabled(false);
                StartCoroutine(CheckActionReady(action, actionButton));
            }

            // Apply styles
            actionButton.AddToClassList("actionButton");
            actionLabel.AddToClassList("actionLabel");
            actionIcon.AddToClassList("actionIcon");

            // Add image to icon
            actionIcon.style.backgroundImage = new StyleBackground(action.icon);

            // Add text to label
            actionLabel.text = action.name;

            // Add label and icon to the button
            actionButton.Add(actionIcon);
            actionButton.Add(actionLabel);

            // Add functionality to the button
            actionButton.clicked += action.Execute;
            actionButton.clicked += delegate { DisableButton(actionButton, action.cooldown); };

            // Add button to the actionsBox
            actionsBox.Add(actionButton);
        }
    }

    public void ClearActionsDisplay()
    {
        actionsBox.Clear();
    }

    public void DisableButton(Button button, float cooldown)
    {
        button.SetEnabled(false);
        StartCoroutine(EnableButton(button, cooldown));
    }

    public IEnumerator EnableButton(Button button, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        button.SetEnabled(true);

        yield return null;
    }

    public IEnumerator CheckActionReady(ActionData action, Button button)
    {
        while (true)
        {
            if (action.readyToUse)
            {
                button.SetEnabled(true);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }
}