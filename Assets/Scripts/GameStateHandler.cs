using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    [Header("Singleton")]
    public static GameStateHandler instance;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;
    }

    public void PlayerVictory()
    {
        UnitSelection.instance.StopAllUnits();
        //GuiHandler.instance.victoryDocument.SetEnabled(true);
        GuiHandler.instance.victoryDocument.SetActive(true);
    }

    public void PlayerDefeat()
    {
        UnitSelection.instance.StopAllUnits();
        //GuiHandler.instance.defeatDocument.SetEnabled(true);
        GuiHandler.instance.defeatDocument.SetActive(true);
    }
}
