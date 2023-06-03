using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class SpeedRunTimer : MonoBehaviour
{
    private bool stopwatchActive = false;
    private float currentTime;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Countdown countdown;
    public float CurrentTime { get { return currentTime; } }

    void Start()
    {
        currentTime = 0;
        countdown.countdownEnd += CountdownEnded;
    }

    private void CountdownEnded()
    {
        stopwatchActive = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) stopwatchActive = !stopwatchActive;

        if (stopwatchActive)
        {
            currentTime = currentTime - Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timeText.text = time.ToString(@"mm\:ss\:ff");
    }

    public void StartTimer()
    {
        currentTime = 0;
        stopwatchActive = true;
    }

    public void StopTimer()
    {
        stopwatchActive = false;
    }
}
