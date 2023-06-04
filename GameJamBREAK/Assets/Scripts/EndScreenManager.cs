using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Stuff to subscribe to")]
    [SerializeField] private string Example2 = "OnWin";
    [SerializeField] private HighScoreManager highScoreManager;

    [Header("Panels")]
    [SerializeField] private GameObject MainHud;
    [SerializeField] private GameObject EndScreen;
    [SerializeField] private Button nextLevelBtn;

    private float bestTimeFloat;
    void Start()
    {
        MainHud.SetActive(true);
        EndScreen.SetActive(false);
        bestTimeFloat = highScoreManager.GetBestTime(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Level4")
        {
            nextLevelBtn.enabled = false;
        }
    }

    public void OnWin()
    {
        headerText.text = OnWinText.ToUpper();
        if (speedRunTimer.CurrentTime < bestTimeFloat)
        {
            bestTime.text = speedRunTimer.CurrentTime.ToString(@"mm\:ss\:ff");
            yourTime.text = speedRunTimer.CurrentTime.ToString(@"mm\:ss\:ff");
            highScoreManager.SaveTime(SceneManager.GetActiveScene().name, speedRunTimer.CurrentTime);
        }
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(bestTimeFloat);
            bestTime.text = time.ToString(@"mm\:ss\:ff");
            yourTime.text = speedRunTimer.CurrentTime.ToString(@"mm\:ss\:ff");

        }
        MainHud.SetActive(false);
        EndScreen.SetActive(true);

    }

}
