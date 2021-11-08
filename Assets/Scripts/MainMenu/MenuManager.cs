using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonsParent;

    public GameObject LeaderboardItemPrefab;
    public Transform LeaderboardItemContainer;

    public string levelsData;

    public Image selectedLevelImage;

    void Start()
    {
        LoadLevels();
    }

    public void LoadLevels()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                if (result.Data == null || !result.Data.ContainsKey("levelsdata")) Debug.Log("No Level Data");
                else
                {
                    levelsData = result.Data["levelsdata"];
                    Debug.Log(levelsData);
                    ParsedJSONClass p = ParsedJSONClass.CreateFromJSON(levelsData);
                    foreach (LevelData ld in p.levels)
                    {
                        Debug.Log(LeaderboardItemPrefab == null);
                        GameObject instantiatedPrefab = Instantiate(LeaderboardItemPrefab, LeaderboardItemContainer);
                        instantiatedPrefab.GetComponent<LevelButtonContainer>().UpdateContainer(ld.levelname, ld.levelbitmap, ld.levelimage);
                    }
                }
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }

}


[System.Serializable]
public class ParsedJSONClass
{
    public LevelData[] levels;

    public static ParsedJSONClass CreateFromJSON(string JsonString)
    {
        return JsonUtility.FromJson<ParsedJSONClass>(JsonString);
    }
}

[System.Serializable]
public class LevelData
{
    public string levelname, levelbitmap, levelimage;
}