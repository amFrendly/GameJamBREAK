using System;
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
    [SerializeField] private TextMeshProUGUI newBest;

    [Header("Stuff to subscribe to")]
    [SerializeField] private HighScoreManager highScoreManager;

    [Header("Panels")]
    [SerializeField] private GameObject MainHud;
    [SerializeField] private GameObject EndScreen;
    [SerializeField] private Button nextLevelBtn;
    [Header("Player prefab")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject eventSystem;

    private float bestTimeFloat;
    void Start()
    {
        eventSystem.GetComponent<MouseControllSystem>().enabled = false;
        MainHud.SetActive(true);
        EndScreen.SetActive(false);
        newBest.enabled = false;
        bestTimeFloat = highScoreManager.GetBestTime(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Level4")
        {
            nextLevelBtn.enabled = false;
        }
    }

    public void OnWin()
    {
        eventSystem.GetComponent<MouseControllSystem>().enabled = true;
        Time.timeScale = 0;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<Gun>().enabled = false;
        player.GetComponent<Grapple>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        headerText.text = OnWinText.ToUpper();
        if (speedRunTimer.CurrentTime < bestTimeFloat)
        {
            newBest.enabled = true;
            TimeSpan time = TimeSpan.FromSeconds(speedRunTimer.CurrentTime);
            bestTime.text = time.ToString(@"mm\:ss\:ff");
            yourTime.text = time.ToString(@"mm\:ss\:ff");
            highScoreManager.SaveTime(SceneManager.GetActiveScene().name, speedRunTimer.CurrentTime);
        }
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(bestTimeFloat);
            bestTime.text = time.ToString(@"mm\:ss\:ff");
            TimeSpan youTime = TimeSpan.FromSeconds(speedRunTimer.CurrentTime);
            yourTime.text = youTime.ToString(@"mm\:ss\:ff");

        }
        MainHud.SetActive(false);
        EndScreen.SetActive(true);
    }

}
