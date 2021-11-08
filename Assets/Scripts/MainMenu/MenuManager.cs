using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonsParent;

    public GameObject LeaderboardItemPrefab;
    public Transform LeaderboardItemContainer;

    [HideInInspector]public string levelsData;
    [HideInInspector]public Image selectedLevelImage;

    public GameObject levelsLoadingPanel;

    [Space(15f)]
    public TMP_Text usernameText;
    public TMP_Text coinText;

    void Start()
    {
        LoadLevels();
        Debug.Log(GeneralInfo.Instance.username);
        usernameText.text = GeneralInfo.Instance.username;
        coinText.text = GeneralInfo.Instance.coinCount.ToString();
    }

    public void LoadLevels()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                if (result.Data == null || !result.Data.ContainsKey("levelsdata")) Debug.Log("No Level Data");
                else
                {
                    levelsLoadingPanel.SetActive(true);
                    levelsData = result.Data["levelsdata"];
                    Debug.Log(levelsData);
                    ParsedJSONClass p = ParsedJSONClass.CreateFromJSON(levelsData);
                    StartCoroutine(LoadLevelButtons(p));
                }
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }

    IEnumerator LoadLevelButtons(ParsedJSONClass p)
    {
        foreach (LevelData ld in p.levels)
        {
            GameObject instantiatedPrefab = Instantiate(LeaderboardItemPrefab, LeaderboardItemContainer);
            yield return StartCoroutine(instantiatedPrefab.GetComponent<LevelButtonContainer>().UpdateContainer(ld.levelname, ld.levelbitmap, ld.levelimage));
        }
        levelsLoadingPanel.SetActive(false);
    }

    public void UpdateCoin()
    {
        coinText.text = GeneralInfo.Instance.coinCount.ToString();
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