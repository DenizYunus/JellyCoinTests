using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayFabCommonTasks : MonoBehaviour
{
    public static PlayFabCommonTasks Instance = null;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCoin(int changeCoin)
    {
        int coinCount = -1;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() //GET USER DATA
        {
            PlayFabId = GeneralInfo.Instance.playFabId,
            Keys = null
        }, result =>
        { //GOT USER DATA
            if (result.Data == null || !result.Data.ContainsKey("CoinCount"))
            {
                Debug.Log("No Coin Count found.");
            }
            else
            {
                coinCount = Convert.ToInt32(result.Data["CoinCount"].Value);

                if (coinCount != -1)
                {
                    PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
                    {
                        Data = new Dictionary<string, string>() { { "CoinCount", (coinCount + changeCoin).ToString() } } // CREATE FIRST COIN ON PLAYFAB
                    }, result => { GetCoinCount(); GeneralInfo.Instance.coinCount = coinCount + changeCoin; MenuManager.Instance.UpdateCoin(); }, error => { Debug.LogError("Error: " + error.GenerateErrorReport()); });
                }
            }
        }, error => Debug.LogError("Error encountered: " + error.GenerateErrorReport()));
    }

    public void GetCoinCount()
    {
        int coinCount = -1;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() //GET USER DATA
        {
            PlayFabId = GeneralInfo.Instance.playFabId,
            Keys = null
        }, result =>
        { //GOT USER DATA
            if (result.Data == null || !result.Data.ContainsKey("CoinCount"))
            {
                Debug.Log("No Coin Count found.");
            }
            else
            {
                coinCount = Convert.ToInt32(result.Data["CoinCount"].Value);
                GeneralInfo.Instance.coinCount = coinCount;
            }
        }, error => { Debug.LogError("Error: " + error.GenerateErrorReport()); });
    }
}
