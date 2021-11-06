using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTouch : MonoBehaviour
{
    public static int CurrentStep = 60;
    public int InternalStep;
    public Text Steps;
    bool reachTheZero = false; 

    public void Update()
    {
        if (reachTheZero == false)
        {
            InternalStep = CurrentStep;
            Steps.text = "" + InternalStep;
            if (CurrentStep == 0)
            {
                Debug.Log("Your steps finish!");
                reachTheZero = true;
            }
        }
    }
}
