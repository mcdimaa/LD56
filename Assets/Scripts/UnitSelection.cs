using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class UnitSelection : MonoBehaviour
{
    [Header("Singleton")]
    public static UnitSelection instance;

    [Header("Selection Box")]
    public Rect boxSelection;
    public Vector2 boxStartPosition;
    public Vector2 boxEndPosition;

    [Header("UI References")]
    public GameObject boxSelectVisualObject;
    public VisualElement boxSelectionDocument;
    public VisualElement boxSelectionVisual;

    [Header("Lists")]
    public List<RtsObject> rtsObjects;
    public List<RtsObject> selectedObjects;

    [Header("Other")]
    private bool moveIndicatorReady;
    private bool gatherIndicatorReady;
    public float indicatorCooldown;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;

        // Initialise lists
        rtsObjects = new List<RtsObject>();
        selectedObjects = new List<RtsObject>();

        // Initialise other variables
        boxSelection = new Rect();
        boxStartPosition = Vector2.zero;
        boxEndPosition = Vector2.zero;
        moveIndicatorReady = true;
        gatherIndicatorReady = true;

        // Set other references
        boxSelectVisualObject.SetActive(true);
        boxSelectionDocument = boxSelectVisualObject.GetComponent<UIDocument>().rootVisualElement;
        boxSelectionVisual = boxSelectionDocument.Q<VisualElement>("BoxSelection");
    }

    private void Start()
    {
        // Add all units to the units list
        foreach (RtsObject rtsObject in FindObjectsByType(typeof(RtsObject), FindObjectsSortMode.None))
        {
            rtsObjects.Add(rtsObject);
        }

        // Initialise box selection visual
        UpdateBoxVisual();
    }

    private void Update()
    {
        // If player has pressed the selection key
        if (Input.GetKeyUp(Keybinds.instance.selectKey))
        {
            // Make sure user is not clicking on the UI (if they are, ignore unit command)
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Shoot a raycast from camera to clicked point (mouse position)
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Hit a clickable object
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, GlobalReferences.instance.selectableMask))
                {

                    if (Input.GetKey(Keybinds.instance.multipleSelectKey))
                    {
                        RtsObject hitObject = hit.collider.GetComponent<RtsObject>();

                        SelectMultipleUnits(hitObject);
                    }
                    else
                    {
                        RtsObject hitObject = hit.collider.GetComponent<RtsObject>();

                        SelectUnit(hitObject);

                        GuiHandler.instance.ClearActionsDisplay();
                        GuiHandler.instance.ClearDisplayInfo();
                        GuiHandler.instance.DisplayInfo(hitObject.GetObjectInfo());

                        if (hitObject.actions.Count != 0)
                        {
                            GuiHandler.instance.ClearActionsDisplay();
                            GuiHandler.instance.DisplayActions(hitObject.GetObjectActions());
                        }
                    }
                }
                else // Didn't hit a clickable object
                {
                    if (!Input.GetKey(Keybinds.instance.multipleSelectKey))
                    {
                        DeselectAll();

                        GuiHandler.instance.ClearDisplayInfo();
                        GuiHandler.instance.ClearActionsDisplay();
                    }
                }
            }
        }

        // Handle unit selection box
        if (Input.GetKeyDown(Keybinds.instance.selectKey))
        {
            boxStartPosition = GetActualMousePos();
            boxSelection = new Rect();
        }
        if (Input.GetKey(Keybinds.instance.selectKey))
        {
            boxEndPosition = GetActualMousePos();
            UpdateBoxVisual();
            UpdateBoxSelection();
        }
        if (Input.GetKeyUp(Keybinds.instance.selectKey))
        {
            DragSelect();
            boxStartPosition = Vector2.zero;
            boxEndPosition = Vector2.zero;
            UpdateBoxVisual();
        }
    }

    /// <summary>
    /// Updates the visual showing where the player has selected
    /// </summary>
    private void UpdateBoxVisual()
    {
        // Update size
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStartPosition.x - boxEndPosition.x), Mathf.Abs(boxStartPosition.y - boxEndPosition.y));
        boxSelectionVisual.style.width = boxSize.x;
        boxSelectionVisual.style.height = boxSize.y;

        // Update position
        Vector2 boxCenter = (boxStartPosition + boxEndPosition) / 2;
        boxSelectionVisual.style.left = boxCenter.x - (boxSelectionVisual.style.width.value.value / 2);
        boxSelectionVisual.style.top = boxCenter.y - (boxSelectionVisual.style.height.value.value / 2);
    }

    /// <summary>
    /// Updates the actual box selection created by player
    /// </summary>
    private void UpdateBoxSelection()
    {
        if (GetActualMousePos().x < boxStartPosition.x)
        {
            // Dragging left
            boxSelection.xMin = GetActualMousePos().x;
            boxSelection.xMax = boxStartPosition.x;
        }
        else
        {
            // Dragging right
            boxSelection.xMin = boxStartPosition.x;
            boxSelection.xMax = GetActualMousePos().x;
        }

        if (GetActualMousePos().y < boxStartPosition.y)
        {
            // Dragging down
            boxSelection.yMin = GetActualMousePos().y;
            boxSelection.yMax = boxStartPosition.y;
        }
        else
        {
            // Dragging up
            boxSelection.yMin = boxStartPosition.y;
            boxSelection.yMax = GetActualMousePos().y;
        }
    }

    /// <summary>
    /// Displays the move location indicator at the specified location
    /// </summary>
    /// <param name="location">Where to show it</param>
    public void ShowMoveLocationIndicator(Vector3 location)
    {
        if (moveIndicatorReady)
        {
            moveIndicatorReady = false;
            Instantiate(GlobalReferences.instance.moveLocationIndicator, location, new Quaternion(0, 0, 0, 0));
            Invoke("ReadyMoveIndicator", indicatorCooldown);
        }
    }
    
    /// <summary>
    /// Makes the move indicator ready to use again
    /// </summary>
    private void ReadyMoveIndicator()
    {
        moveIndicatorReady = true;
    }

    /// <summary>
    /// Displays the gather indicator at the specified location
    /// </summary>
    /// <param name="location">Where to show it</param>
    public void ShowGatherIndicator(Vector3 location)
    {
        if (gatherIndicatorReady)
        {
            gatherIndicatorReady = false;
            Instantiate(GlobalReferences.instance.gatherIndicator, location, new Quaternion(0, 0, 0, 0));
            Invoke("ReadyGatherIndicator", indicatorCooldown);
        }
    }

    /// <summary>
    /// Makes the gather indicator ready to use again
    /// </summary>
    private void ReadyGatherIndicator()
    {
        gatherIndicatorReady = true;
    }

    /// <summary>
    /// Gets the proper mouse position on UI Toolkit canvas
    /// </summary>
    /// <returns>The mouse position</returns>
    private Vector2 GetActualMousePos()
    {
        var canvasSize = boxSelectionDocument.resolvedStyle;
        float xPos = Input.mousePosition.x * canvasSize.width / Screen.width;
        float yPos = canvasSize.height - (Input.mousePosition.y * canvasSize.height / Screen.height);

        return new Vector2(xPos, yPos);
    }

    /// <summary>
    /// Adds a provided unit to the units list
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnit(RtsObject rtsObject)
    {
        rtsObjects.Add(rtsObject);
    }

    /// <summary>
    /// Selects a single provided unit
    /// </summary>
    /// <param name="unit">The unit to select</param>
    public void SelectUnit(RtsObject rtsObject)
    {
        DeselectAll();
        selectedObjects.Add(rtsObject);
        rtsObject.Select();
    }

    /// <summary>
    /// Adds a provided unit to the selection
    /// </summary>
    /// <param name="unit">The unit to select</param>
    public void SelectMultipleUnits(RtsObject rtsObject)
    {
        if (!selectedObjects.Contains(rtsObject))
        {
            selectedObjects.Add(rtsObject);
            rtsObject.Select();
        }
        else
        {
            selectedObjects.Remove(rtsObject);
            rtsObject.Deselect();
        }
    }

    /// <summary>
    /// Selects every unit within the box selection area
    /// </summary>
    public void DragSelect()
    {
        foreach (RtsObject rtsObject in rtsObjects)
        {
            // Calculate proper position based on canvas/screen size
            var canvasSize = boxSelectionDocument.resolvedStyle;

            var point = Camera.main.WorldToScreenPoint(rtsObject.transform.position);

            point.x = point.x * canvasSize.width / Screen.width;
            point.y = canvasSize.height - (point.y * canvasSize.height / Screen.height);

            if (boxSelection.Contains(point))
            {
                // If unit isn't already selected
                if (!selectedObjects.Contains(rtsObject))
                {
                    // Add it to selection
                    selectedObjects.Add(rtsObject);
                    rtsObject.Select();
                }
            }
        }
    }

    /// <summary>
    /// Deselects the provided unit
    /// </summary>
    /// <param name="unit">The unit to deselect</param>
    public void DeselectUnit(Unit unit)
    {
        selectedObjects.Remove(unit);
        unit.Deselect();
    }

    /// <summary>
    /// Deselects all selected units
    /// </summary>
    public void DeselectAll()
    {
        foreach (RtsObject rtsObject in selectedObjects)
        {
            if (rtsObject != null)
                rtsObject.Deselect();
        }
        selectedObjects.Clear();
    }
}
