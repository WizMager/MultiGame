using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLoginRobot : MonoBehaviour
{
    private const string AuthorizedKey = "AuthorizedKey";
    private string _playFabId;
    private const string HealthKey = "HealthKey";
    private Dictionary<string, string> _health;

    private void Start()
    {
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
        Debug.Log("Login is success!");
        _playFabId = result.PlayFabId;
        SetRobotHealth();
    }

    private void SetRobotHealth()
    {
        _health = new Dictionary<string, string>{{HealthKey, "1"}};
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            
            Data = _health
        }, _ => { }, OnLoginError);
    }

    public void TakeDamage(float damage)
    {
        var currentHealth = float.Parse(_health[HealthKey]);
        currentHealth -= damage;
        _health[HealthKey] = currentHealth.ToString();
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = _health
        }, _ => {}, OnLoginError);
    }

    public float GetHealth()
    {
        float health = 0;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _playFabId
        }, result => {
            if (result.Data.ContainsKey(HealthKey))
            {
                health = float.Parse(result.Data[HealthKey].Value);
            }
        }, OnLoginError);
        return health;
    }
    
    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log(errorMessage);
    }
}