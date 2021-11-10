using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private static float startTime;
    public static bool timerRunning = false;

    public static float timePassed;

    public static void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
    }

    void Update()
    {
        if (timerRunning)
        {
            timePassed = Time.time - startTime;

            string minutes = ((int)timePassed / 60).ToString();
            string seconds = (timePassed % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;
        }
    }
}
