using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpdateLevelsText : MonoBehaviour
{
    [Header("Button Time Text")]
    [SerializeField] private TextMeshProUGUI[] levelsText;
    [SerializeField] private string[] sceneNames;
    [Space]
    [SerializeField] private HighScoreManager highScoreManager;
    private TimeSpan[] levelTime;
    private TimeSpan CheckTime = TimeSpan.FromSeconds(600);
    void Start()
    {
        levelTime = new TimeSpan[levelsText.Count()];

        for (int i = 0; i < levelsText.Count(); i++)
        {
            if (levelTime[i] == CheckTime)
            {
                levelsText[i].text = "";
            }
            else
            {
                levelTime[i] = TimeSpan.FromSeconds(highScoreManager.GetBestTime(sceneNames[i]));
                levelsText[i].text = levelTime[i].ToString(@"mm\:ss\:ff");
                levelsText[i].text = "";
            }
        }
    }
}
