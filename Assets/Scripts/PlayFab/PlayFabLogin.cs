using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class PlayFabLogin : MonoBehaviour
{
    public TMP_InputField loginEmailText;
    public TMP_InputField loginPasswordText;

    public TMP_InputField registerUsernameText;
    public TMP_InputField registerEmailText;
    public TMP_InputField registerPasswordText;

    public TMP_Text commonNotificationText;

    public Button loginButton;
    public Button registerButton;

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "322FD"; // titleId from PlayFab Game Manager
        }
    }

    public void LoginWithEmail()
    {
        loginButton.enabled = false;
        var request = new LoginWithEmailAddressRequest { Email = loginEmailText.text, Password = loginPasswordText.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void RegisterWithEmailUsername()
    {
        loginButton.enabled = false;
        var request = new RegisterPlayFabUserRequest { Email = registerEmailText.text, Username = registerUsernameText.text, Password = registerPasswordText.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }


    private void OnLoginSuccess(LoginResult loginResult)
    {
        GeneralInfo.Instance.email = loginEmailText.text;

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() //GET PLAYER PROFILE
        {
            PlayFabId = loginResult.PlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        }, result => {  //GOT PLAYER PROFILE
            GeneralInfo.Instance.username = result.PlayerProfile.DisplayName;

            PlayFabClientAPI.GetUserData(new GetUserDataRequest() //GET USER DATA
            {
                PlayFabId = loginResult.PlayFabId,
                Keys = null
            }, result => { //GOT USER DATA
                if (result.Data == null || !result.Data.ContainsKey("CoinCount"))
                {
                    PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
                    {
                        Data = new Dictionary<string, string>() { { "CoinCount", "0" } } // CREATE FIRST COIN ON PLAYFAB
                    }, result => { GeneralInfo.Instance.coinCount = 0; }, error => { Debug.Log("Error: " + error.GenerateErrorReport()); });
                }

                else GeneralInfo.Instance.coinCount = Convert.ToInt32(result.Data["CoinCount"].Value); // GET PRE-CREATED COIN FROM PLAYFAB
            }, (error) => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });

            commonNotificationText.text = "Login Successful";

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }, error => Debug.Log("Error: " + error.GenerateErrorReport()));
    }

    private void OnLoginFailure(PlayFabError error)
    {
        loginButton.enabled = true;
        commonNotificationText.text = error.ErrorMessage + " :(";
        commonNotificationText.transform.parent.gameObject.SetActive(true);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        GeneralInfo.Instance.email = registerEmailText.text;
        GeneralInfo.Instance.username = registerUsernameText.text;
        GeneralInfo.Instance.coinCount = 0;

        commonNotificationText.text = "Register Successful";

        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = registerUsernameText.text };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result => { Debug.Log("User Title is " + registerUsernameText.text); }, error => Debug.Log("Update display name error: " + error.GenerateErrorReport()));
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        loginButton.enabled = true;
        commonNotificationText.text = error.ErrorMessage + " :(";
        commonNotificationText.transform.parent.gameObject.SetActive(true);
    }
}
