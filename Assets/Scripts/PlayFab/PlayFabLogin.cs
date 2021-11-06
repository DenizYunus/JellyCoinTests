using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    public InputField loginEmailText;
    public InputField loginPasswordText;

    public InputField registerUsernameText;
    public InputField registerEmailText;
    public InputField registerPasswordText;

    public Text commonNotificationText;

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "322FD"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        
        //var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    public void LoginWithEmail()
    {
        var request = new LoginWithEmailAddressRequest { Email = loginEmailText.text, Password = loginPasswordText.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void RegisterWithEmailUsername()
    {
        var request = new RegisterPlayFabUserRequest { Email = registerEmailText.text, Username = registerUsernameText.text, Password = registerPasswordText.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }


    private void OnLoginSuccess(LoginResult result)
    {
        commonNotificationText.text = "Login Successful";        
    }

    private void OnLoginFailure(PlayFabError error)
    {
        commonNotificationText.text = error.ErrorMessage;
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        commonNotificationText.text = "Register Successful";

        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = registerUsernameText.text };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result => { Debug.Log("User Title is " + registerUsernameText.text); }, error => Debug.Log("Update display name error: " + error.GenerateErrorReport()));
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        commonNotificationText.text = error.ErrorMessage;
    }
}
