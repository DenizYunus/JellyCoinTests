using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JSONDataParser;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject obstaclePrefab;

    void Start()
    {
        foreach (ObjectTransform objt in MenuManager.Instance.selectedLevelMapDetails.players)
        {
            Instantiate(playerPrefab, new Vector3(objt.x, objt.y, objt.z), Quaternion.Euler(objt.rx, objt.ry, objt.rz));
        }

        foreach (ObjectTransform objt in MenuManager.Instance.selectedLevelMapDetails.obstacles)
        {
            Instantiate(obstaclePrefab, new Vector3(objt.x, objt.y, objt.z), Quaternion.Euler(objt.rx, objt.ry, objt.rz));
        }
    }

    public void ChangeScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }
}
