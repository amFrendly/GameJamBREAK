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

    private bool stopwatchActive = false;
    private float currentTime = 3;

    void Start()
    {
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
            currentTime = currentTime - Time.fixedDeltaTime;
            if (currentTime < 0)
                countdownEnd?.Invoke();
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        countText.text = time.Seconds.ToString();
    }
}
