using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Singleton")]
    public static MapController instance;

    [Header("Map Values")]
    public float mapSize;

    [Header("Other References")]
    public Texture mapTexture;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        UpdateMapSize();
    }

    /// <summary>
    /// Updates the map object's size based on the mapSize value
    /// </summary>
    private void UpdateMapSize()
    {
        Vector3 newSize = Vector3.one;
        newSize.x = mapSize;
        newSize.z = mapSize;
        transform.localScale = newSize;
    }
}
