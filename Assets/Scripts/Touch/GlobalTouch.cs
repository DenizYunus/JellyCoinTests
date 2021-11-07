using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTouch : MonoBehaviour
{
    public static int CurrentStep = 30;
    public int InternalStep;
    public Text Steps;
    public static bool countingTouches = false;

    public void Update()
    {
        if (countingTouches == true)
        {
            InternalStep = CurrentStep;
            Steps.text = "" + InternalStep;
        }
    }
}
