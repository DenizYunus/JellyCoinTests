using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HardTransforms : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject notificationLoadingPanel;
    public Transform LevelButtonsParent;
    public Transform LeaderboardButtonsParent;
    public Transform AnnouncementsParent;

    public Image selectedLevelImage;

    [Space(15f)]
    public TMP_Text usernameText;
    public TMP_Text coinText;

    public void ChangeScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }

    public void UpdateCoin(int coinCount)
    {
        PlayFabCommonTasks.Instance.UpdateCoin(coinCount);
    }

    public void UpdateNotifications()
    {
        MenuManager.Instance.UpdateAnnouncements();
    }
}
