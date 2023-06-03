using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{

    public delegate void OnCountdownEnd();
    public OnCountdownEnd countdownEnd;

    [Header("Subscribe")]
    [SerializeField] private LevelLoader lvlLoader;

    [Header("Count Text")]
    [SerializeField] private TextMeshProUGUI countText;

    [Header("Player")]
    [SerializeField] private GameObject player;

    private bool stopwatchActive = false;
    private float currentTime = 3;

    private void Awake()
    {
        Time.timeScale = 0;
        
    }
    void Start()
    {
        player.GetComponent<Gun>().enabled = false;
        player.GetComponent<Grapple>().enabled = false;
        Time.timeScale = 0;
        stopwatchActive = false;
        countText.enabled = true;
        lvlLoader.fadeEnd += FadeEnd;
        countText.text = "3";
        countdownEnd += countdownEnds;
    }

    private void countdownEnds()
    {
        stopwatchActive = false;
        countText.enabled = false;
        player.GetComponent<Gun>().enabled = true;
        player.GetComponent<Grapple>().enabled = true;
        Time.timeScale = 1;
    }
    private void FadeEnd()
    {
        stopwatchActive = true;
    }
    void Update()
    {
        if (stopwatchActive)
        {
            currentTime = currentTime - Time.unscaledDeltaTime;
            if (currentTime < 0)
                countdownEnd?.Invoke();
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        countText.text = time.Seconds.ToString();
    }
}
