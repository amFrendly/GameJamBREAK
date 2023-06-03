using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panels : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    [SerializeField] private GameObject Levels;

    public void SwitchPanels(string activePanelName)
    {
        switch (activePanelName)
        {
            case "Main":
                Main.SetActive(false);
                Levels.SetActive(true);
                break;
            case "Levels":
                Levels.SetActive(false);
                Main.SetActive(true);
                break;
        }
    }

}
