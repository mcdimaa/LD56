using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybinds : MonoBehaviour
{
    [Header("Singleton")]
    public static Keybinds instance;

    [Header("Unit Controls")]
    public KeyCode selectKey;
    public KeyCode multipleSelectKey;
    public KeyCode actionKey;
    [Header("Camera Controls")]
    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;
    }
}
