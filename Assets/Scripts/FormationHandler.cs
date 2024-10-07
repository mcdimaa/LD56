using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FormationHandler : MonoBehaviour
{
    [Header("Singleton")]
    public static FormationHandler instance;

    [Header("References")]
    //public Dictionary<RtsObject, List<Vector3>> rtsObjectsDict;
    public Dictionary<RtsObject, Dictionary<Vector3, bool>> rtsObjectsDict;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null )
            instance = this;

        // Intialise unitsList
        //rtsObjectsDict = new Dictionary<RtsObject, List<Vector3>>();
        rtsObjectsDict = new Dictionary<RtsObject, Dictionary<Vector3, bool>>();
    }

    private void Start()
    {
        // Add all existing rtsObjects to list
        foreach (RtsObject rtsObject in FindObjectsByType(typeof(RtsObject), FindObjectsSortMode.None))
        {
            //rtsObjectsDict.Add(rtsObject, GetPositionsAroundObject(rtsObject));

            Dictionary<Vector3, bool> positions = new Dictionary<Vector3, bool>();
            foreach (Vector3 pos in GetPositionsAroundObject(rtsObject))
            {
                positions.Add(pos, false);
            }
            rtsObjectsDict.Add(rtsObject, positions);
        }
    }

    /// <summary>
    /// Calculates the number of positions around the provided object, returning a list of each position
    /// </summary>
    /// <param name="rtsObject">The object to check</param>
    /// <returns>A list of all positions</returns>
    public List<Vector3> GetPositionsAroundObject(RtsObject rtsObject)
    {
        // Initialise variables
        float a; // Side length of each square (unit in this case)
        float b; // Radius of circle (object to form around)
        float c; // Circumference of circle
        float n; // Approximate number of squares that can fit around circle

        // Get the object's actual size
        Vector3 objectSize = rtsObject.transform.lossyScale;

        // Assuming using squares, A is one side length of the square (multiplied by 2 for spacing)
        a = GlobalReferences.instance.workerCreature.transform.lossyScale.x * 2;
        // B is whichever length is longer since we want to form a circle, even if not quite even sides
        if (objectSize.x >= objectSize.z) b = objectSize.x / 2; else b = objectSize.z / 2;
        // Circumference C = 2 * PI * B
        c = (float)(2 * Mathf.PI * b);

        // Approximate number of squares N = C / A
        n = c / a;

        // Round N to whole number
        n = Mathf.FloorToInt(n);

        // Initialise list of positions
        List<Vector3> positions = new List<Vector3>();

        float angleStep = 2 * Mathf.PI / n;

        for (int i = 0; i < n; i++)
        {
            float angle = i * angleStep;

            float x = b * Mathf.Cos(angle);
            float z = b * Mathf.Sin(angle);

            Vector3 pos = new Vector3(x, 0, z) + rtsObject.transform.position;
            positions.Add(pos);
        }

        return positions;
    }

    /// <summary>
    /// Gets an available position from the provided rtsObject if there is one, and sets it as unavailable
    /// </summary>
    /// <param name="rtsObject">The object to get a position around</param>
    /// <returns>The available position</returns>
    public Vector3 GetDestination(RtsObject rtsObject)
    {
        if (rtsObjectsDict.TryGetValue(rtsObject, out var positionsDict))
        {
            for (int i = 0; i < positionsDict.Count; i++)
            {
                if (positionsDict.ElementAt(i).Value) // True means position is occupied
                {
                    continue;
                }
                else // Unoccupied
                {
                    // Set to true, it is now occupied
                    positionsDict[positionsDict.ElementAt(i).Key] = true;
                    return positionsDict.ElementAt(i).Key;
                }
            }
            Debug.Log("Object is full, no positions left");
        }
        else
        {
            Debug.Log("RtsObject was not found in dictionary");
        }
        return Vector3.zero;
    }

    public void FreePosition(Resource resource, Vector3 position)
    {
        if (resource != null)
        {
            rtsObjectsDict.TryGetValue(resource.GetComponent<RtsObject>(), out var positionsDict);
            positionsDict[positionsDict.First(t => t.Key == position).Key] = false;
        }
    }
}
