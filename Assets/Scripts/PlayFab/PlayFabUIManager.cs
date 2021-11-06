using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabUIManager : MonoBehaviour
{
    public Transform leaderboardContainer;
    public GameObject leaderboardItemContainerPrefab;

    void Update()
    {
        
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "CompleteTime", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGot, OnLeaderboardError);
    }

    private void OnLeaderboardGot(GetLeaderboardResult result)
    {
        foreach (Transform item in leaderboardContainer)
        {
            Destroy(item.gameObject);
        }

        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject newItem = Instantiate(leaderboardItemContainerPrefab, leaderboardContainer);
            newItem.GetComponent<LeaderboardItemContainer>().setItem(player.DisplayName, player.StatValue.ToString());
        }
    }

    private void OnLeaderboardError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
