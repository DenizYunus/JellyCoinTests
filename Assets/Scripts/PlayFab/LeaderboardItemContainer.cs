using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItemContainer : MonoBehaviour
{
    string username;
    string score;

    public void setItem(string _username, string _score)
    {
        username = _username;
        score = _score;
        transform.GetChild(0).GetComponent<Text>().text = username;
        transform.GetChild(1).GetComponent<Text>().text = score;
    }
}
