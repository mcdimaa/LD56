using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    /// <summary>
    /// Destroys the indicator gameObject
    /// </summary>
    public void DestroyIndicator()
    {
        Destroy(gameObject);
    }
}
