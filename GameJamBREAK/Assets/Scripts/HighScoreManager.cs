using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour
{
    private float highScore;
    private const string Level1 = "Level1", Level2 = "Level2", Level3 = "Level3", Level4 = "Level4", Tutorial = "Tutorial", FirstTime = "FirstTime";
    private int firstTime;
    private void Awake()
    {
        //firstTime = PlayerPrefs.GetInt(FirstTime);
        firstTime = 0;
        if (firstTime == 0)
        {
            PlayerPrefs.SetFloat(Level1, 600);
            PlayerPrefs.SetFloat(Level2, 600);
            PlayerPrefs.SetFloat(Level3, 600);
            PlayerPrefs.SetFloat(Level4, 600);
            PlayerPrefs.SetFloat(Tutorial, 600);
            PlayerPrefs.SetInt(FirstTime, 1);
        }
    }
    public float GetBestTime(string sceneName)
    {
        switch (sceneName)
        {
            case Level1:
                highScore = (PlayerPrefs.GetFloat(Level1));
                break;
            case Level2:
                highScore = (PlayerPrefs.GetFloat(Level2));
                break;
            case Level3:
                highScore = (PlayerPrefs.GetFloat(Level3));
                break;
            case Level4:
                highScore = (PlayerPrefs.GetFloat(Level4));
                break;
            case Tutorial:
                highScore = (PlayerPrefs.GetFloat(Tutorial));
                break;
        }
        return highScore;
    }

    public void SaveTime(string sceneName, float time)
    {
        switch (sceneName)
        {
            case Level1:
                PlayerPrefs.SetFloat(Level1, time);
                break;
            case Level2:
                PlayerPrefs.SetFloat(Level2, time);
                break;
            case Level3:
                PlayerPrefs.SetFloat(Level3, time);
                break;
            case Level4:
                PlayerPrefs.SetFloat(Level4, time);
                break;
            case Tutorial:
                PlayerPrefs.SetFloat(Tutorial, time); 
                break;
        }
    }

}
