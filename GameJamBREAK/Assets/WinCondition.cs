using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class WinCondition : MonoBehaviour
{
    
    private int condition;
    [SerializeField][Range(0,1)] private float slowMoScale = 1.0f;
    [SerializeField] private EndScreenManager endScreen;
    [SerializeField] private float slowScreenTime = 2.0f;
    [SerializeField] private SpeedRunTimer speedRunTimer;
    [SerializeField] private KillScript killScript;
    //[SerializeField] private KatanaSlicer katanaSlicer;

    //private float timeScale;

    // Start is called before the first frame update
    void Start()
    {
        Destructable[] destructables = GetComponentsInChildren<Destructable>();
        condition = destructables.Length;
        for (int i = 0; i < condition; i++)
        {
            destructables[i].onDestruct += CountDownOne;
        }
       // katanaSlicer.onSlice += CountDownOne;
    }

    private void CountDownOne()
    {
       
        condition--;
       // Debug.Log("CountDownOne " + condition);
        if(condition == 0)
        {
            speedRunTimer.StopTimer();
           // timeScale = Time.timeScale;
            Time.timeScale = slowMoScale;
            Time.fixedDeltaTime = 0.02f * slowMoScale;
            killScript.enabled= false;
            StartCoroutine(ShowEndScreen());
        }
    }

    private IEnumerator ShowEndScreen()
    {
        yield return new WaitForSecondsRealtime(slowScreenTime);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        endScreen.OnWin();
    }
}
