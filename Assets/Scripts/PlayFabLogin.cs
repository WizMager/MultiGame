using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loginLabel;
    private void Start()
    {
        _loginLabel.text = "Login...";
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "28CE7";
        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "CustomId",
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _loginLabel.text = "Login is success!";
        _loginLabel.color = Color.green;
       Debug.Log("Login is success!"); 
    }

    private void OnLoginError(PlayFabError error)
    {
        _loginLabel.text = "Login ERROR!";
        _loginLabel.color = Color.red;
        var errorMessage = error.GenerateErrorReport();
        Debug.Log(errorMessage);
    }
}