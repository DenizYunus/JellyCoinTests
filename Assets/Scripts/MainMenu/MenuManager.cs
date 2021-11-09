using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour//Singleton<MenuManager>
{
    public static MenuManager Instance;

    public GameObject levelButtonPrefab;

    public GameObject LeaderboardItemPrefab;

    [HideInInspector]public string levelsData;

    [HideInInspector] public MapDetails selectedLevelMapDetails;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            LoadLevels();
            Debug.Log(GeneralInfo.Instance.username);
            FindObjectOfType<HardTransforms>().usernameText.text = GeneralInfo.Instance.username;
            FindObjectOfType<HardTransforms>().coinText.text = GeneralInfo.Instance.coinCount.ToString();
        }
    }

    public void LoadLevels()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                if (result.Data == null || !result.Data.ContainsKey("levelsdata")) Debug.Log("No Level Data");
                else
                {
                    GameObject.FindObjectOfType<HardTransforms>().loadingPanel.gameObject.SetActive(true);
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
            GameObject instantiatedLevelButton = Instantiate(levelButtonPrefab, FindObjectOfType<HardTransforms>().LevelButtonsParent);
            yield return StartCoroutine(instantiatedLevelButton.GetComponent<LevelButtonContainer>().UpdateContainer(ld.levelname, ld.levelbitmap, ld.levelimage));
            instantiatedLevelButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                selectedLevelMapDetails = ld.mapDetails;

                foreach (Transform gobj in FindObjectOfType<HardTransforms>().LeaderboardButtonsParent)
                {
                    Destroy(gobj.gameObject);
                }
                PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest { StartPosition = 0, StatisticName = ld.levelname + " time", MaxResultsCount = 20 },
                    result =>
                    {
                        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                        {
                            GameObject instantiatedLeaderBoard = Instantiate(LeaderboardItemPrefab, FindObjectOfType<HardTransforms>().LeaderboardButtonsParent);
                            instantiatedLeaderBoard.GetComponent<LeaderboardItemContainer>().setItem(player.DisplayName, player.StatValue.ToString());
                        }
                    }, error => Debug.LogError("Error: " + error.GenerateErrorReport()));
            });
        }
        GameObject.FindObjectOfType<HardTransforms>().loadingPanel.SetActive(false);
    }

    public void UpdateCoin()
    {
         FindObjectOfType<HardTransforms>().coinText.text = GeneralInfo.Instance.coinCount.ToString();
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
    public MapDetails mapDetails;
}

[System.Serializable]
public class MapDetails
{
    public ObjectTransform[] players, obstacles;
}

[System.Serializable]
public class ObjectTransform
{
    public int x, y, z, rx, ry, rz;
}