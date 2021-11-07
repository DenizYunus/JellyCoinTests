using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    private void OnLoginSuccess(LoginResult result)
    {
        GeneralInfo.email = loginEmailText.text;
        //GeneralInfo.username = result.InfoResultPayload.PlayerProfile.DisplayName;

        commonNotificationText.text = "Login Successful";
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        loginButton.enabled = true;
        commonNotificationText.text = error.ErrorMessage + " :(";
        commonNotificationText.transform.parent.gameObject.SetActive(true);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        GeneralInfo.email = registerEmailText.text;
        GeneralInfo.username = registerUsernameText.text;

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
