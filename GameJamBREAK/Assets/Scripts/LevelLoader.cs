using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("Black Image")]
    [SerializeField] private Image blackOutSquare;
    private string sceneName;
    private const string tutorialDone = "Tutorial";
    private int tutorialDoneInt;
    public delegate void OnFadeEnd();
    public OnFadeEnd fadeEnd;
    private void Awake()
    {
        blackOutSquare.raycastTarget = false;
    }
    void Start()
    {
        if (blackOutSquare == null) Debug.Log("No BlackOut Image");
        if (blackOutSquare.enabled == false) blackOutSquare.enabled = true;
        if (blackOutSquare != null) StartCoroutine(FadeToBlack(false));
    }
    public void MainPlay()
    {
        tutorialDoneInt = PlayerPrefs.GetInt(tutorialDone);
        if (tutorialDoneInt == 0)
        {
            sceneName = "Tutorial";
            PlayerPrefs.SetInt(tutorialDone, 1);
        }
        else
        {
            sceneName = "Level1";
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(FadeToBlack());
    }
    public void NextScene(string sceneName)
    {
        this.sceneName = sceneName;
        if (sceneName == "Menu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        StartCoroutine(FadeToBlack());
    }
    
    public void RestartLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack(bool fadeToBlack = true, int fadeSpeed = 1)
    {
        
        Color objectColor = blackOutSquare.color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (blackOutSquare.color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.fixedDeltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.color = objectColor;
                if (blackOutSquare.color.a >= 1)
                {
                    SceneManager.LoadScene(sceneName);
                }
                yield return null;
            }
        }
        else
        {
            while (blackOutSquare.color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.fixedDeltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.color = objectColor;
                yield return null;
            }
            fadeEnd?.Invoke();
        }
    }
}
