using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayFab;
using PlayFab.ClientModels;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private TouchControls touchControls;

    public Timer timer;

    private void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        if (GlobalTouch.countingTouches == true)
        {
            //Debug.Log("Touch started and have moves." + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
            if (OnStartTouch != null) OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);

            if (Timer.timerRunning)
                GlobalTouch.CurrentStep -= 1;

            if (GlobalTouch.CurrentStep == 0)
            {
                GlobalTouch.countingTouches = false;
                Timer.timerRunning = false;

                PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate> {
                        new StatisticUpdate { StatisticName = "CompleteTime", Value = Mathf.FloorToInt(Timer.timePassed) }, }
                },
                (text) => Debug.Log(text),
                (error) => Debug.Log(error.GenerateErrorReport()));
            }
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("Touch ended" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        if (OnEndTouch != null) OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
    }
}
