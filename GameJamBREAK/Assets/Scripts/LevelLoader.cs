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

    public void NextScene(string sceneName)
    {
        this.sceneName = sceneName;
        StartCoroutine(FadeToBlack());
    }
    
    public void RestartLevel()
    {
        sceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(FadeToBlack());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(FadeToBlack());
        }
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
