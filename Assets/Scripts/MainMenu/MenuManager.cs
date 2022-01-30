using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static JSONDataParser;

public class MenuManager : MonoBehaviour//Singleton<MenuManager>
{
    public static MenuManager Instance;

    public GameObject levelButtonPrefab;

    public GameObject LeaderboardItemPrefab;

    public GameObject AnnouncementItemPrefab;

    [HideInInspector] public string levelsData;

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
            FindObjectOfType<HardTransforms>().usernameText.text = GeneralInfo.Instance.username;
            FindObjectOfType<HardTransforms>().coinText.text = GeneralInfo.Instance.coinCount.ToString();
        }
    }

    public void LoadLevels()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
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
            error =>
            {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }

    public void UpdateAnnouncements()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if (result.Data == null || !result.Data.ContainsKey("announcements")) Debug.Log("No Announcement Data");
                else
                {
                    GameObject.FindObjectOfType<HardTransforms>().notificationLoadingPanel.gameObject.SetActive(true);
                    levelsData = result.Data["announcements"];
                    AnnouncementParser p = AnnouncementParser.CreateFromJSON(levelsData);
                    foreach (AnnouncementDetails ad in p.announcements)
                    {
                        GameObject instantiatedAnnouncement = Instantiate(AnnouncementItemPrefab, FindObjectOfType<HardTransforms>().AnnouncementsParent);
                        instantiatedAnnouncement.GetComponent<AnnouncementContainer>().UpdateAnnouncement(ad.announcement, ad.date);
                    }
                    GameObject.FindObjectOfType<HardTransforms>().notificationLoadingPanel.gameObject.SetActive(false);
                }
            },
            error =>
            {
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
