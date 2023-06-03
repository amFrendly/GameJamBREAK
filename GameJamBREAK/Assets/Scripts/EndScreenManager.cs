using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [Header("Your Time Stuff")]
    [SerializeField] private SpeedRunTimer speedRunTimer;
    [SerializeField] private TextMeshProUGUI yourTime;

    [Header("Best Time Stuff")]
    [SerializeField] private TextMeshProUGUI bestTime;

    [Header("Header Text")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private string OnWinText = "Completed";
    [SerializeField] private string OnDeathText = "You lost";

    [Header("Stuff to subscribe to")]
    [SerializeField] private string Example1 = "OnDeath";
    [SerializeField] private string Example2 = "OnWin";

    [Header("Panels")]
    [SerializeField] private GameObject MainHud;
    [SerializeField] private GameObject EndScreen;

    void Start()
    {
        MainHud.SetActive(true);
        EndScreen.SetActive(false);


    }

    private void OnWin()
    {
        headerText.text = OnWinText.ToUpper();
        yourTime.text = speedRunTimer.CurrentTime.ToString(@"mm\:ss\:ff");
        MainHud.SetActive(false);
        EndScreen.SetActive(true);

    }
    private void OnDeath()
    {
        headerText.text = OnDeathText.ToUpper();
        yourTime.text = speedRunTimer.CurrentTime.ToString(@"mm\:ss\:ff");
        MainHud.SetActive(false);
        EndScreen.SetActive(true);
    }

}
