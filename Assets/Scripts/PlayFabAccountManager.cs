using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playFabId;
    [SerializeField] private TMP_Text createdDate;
    [SerializeField] private Button deleteAccount;
    private string _playFabId;

    private void Start()
    {
        playFabId.text = "Loading...";
        deleteAccount.onClick.AddListener(DeleteAccount);
        deleteAccount.enabled = false;
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }

    private void DeleteAccount()
    {
        PlayFabServerAPI.DeletePlayer(new DeletePlayerRequest()
        {
            PlayFabId = _playFabId
        }, result => { Debug.Log($"Player with ID {_playFabId} is deleted!"); }, error => OnError(error));
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log(errorMessage);
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _playFabId = result.AccountInfo.PlayFabId;
        playFabId.text = $"{result.AccountInfo.Username} PlayFab id: {_playFabId}";
        createdDate.text = $"{result.AccountInfo.Username} created account {result.AccountInfo.Created}";
        deleteAccount.enabled = true;
    }
}