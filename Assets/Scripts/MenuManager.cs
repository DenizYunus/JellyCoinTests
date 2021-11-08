using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class MenuManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonsParent;

    
    void Start()
    {
        LoadLevels();
    }

    void Update()
    {
        
    }

    public void LoadLevels()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                if (result.Data == null || !result.Data.ContainsKey("levelsdata")) Debug.Log("No Level Data");
                else Debug.Log("levelsdata: " + result.Data["levelsdata"]);
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }

}