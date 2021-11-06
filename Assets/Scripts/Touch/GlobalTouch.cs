using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTouch : MonoBehaviour
{
    public static int CurrentStep = 60;
    public int InternalStep;
    public Text Steps;
    public static bool reachedZero = false;

    public void Update()
    {
        if (reachedZero == false)
        {
            InternalStep = CurrentStep;
            Steps.text = "" + InternalStep;
        }
    }
}
