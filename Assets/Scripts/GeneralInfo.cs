using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralInfo : Singleton<GeneralInfo>
{
    public static string username;
    public static string email;

    public static int coinCount;

    public static string playFabId;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}