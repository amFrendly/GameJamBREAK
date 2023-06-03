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
    [Space]
    [Header("Scene")]
    [SerializeField] private string sceneName;

    private void Awake()
    {
        blackOutSquare.raycastTarget = false;
    }
    void Start()
    {
        if (sceneName == null) Debug.Log("No scene to load!!");
        if (blackOutSquare == null) Debug.Log("No BlackOut Image");

        if (blackOutSquare != null && sceneName != null) StartCoroutine(FadeToBlack(false));
    }

    public void NextScene()
    {
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
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

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
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.color = objectColor;
                yield return null;
            }
        }
    }
}
