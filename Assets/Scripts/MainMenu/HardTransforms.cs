using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HardTransforms : MonoBehaviour
{
    public GameObject loadingPanel;
    public Transform LevelButtonsParent;
    public Transform LeaderboardButtonsParent;

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
}
