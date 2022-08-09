using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loginLabel;
    private const string AuthorizedKey = "AuthorizedKey";

    private void Start()
    {
        _loginLabel.text = "Login...";
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "28CE7";
        }

        var needCreation = PlayerPrefs.HasKey(AuthorizedKey);
        var id = PlayerPrefs.GetString(AuthorizedKey, Guid.NewGuid().ToString());
        
        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = needCreation
        };
        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            PlayerPrefs.SetString(AuthorizedKey, id);
            OnLoginSuccess(result);
        }, OnLoginError);
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