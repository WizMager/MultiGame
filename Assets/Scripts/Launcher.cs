using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class Launcher : MonoBehaviourPunCallbacks
{
    //[SerializeField] private Button _loginButton; 
    private TextMeshProUGUI _loginButtonText;
    private bool _isConnected;
    
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        //_loginButtonText = _loginButton.GetComponentInChildren<TextMeshProUGUI>();
        //_loginButtonText.text = "Login";
        //_loginButton.onClick.AddListener(LoginButton);
        //Connect();
    }

    private void LoginButton()
    {
        if (_isConnected)
        {
           Disconnect(); 
        }
        else
        {
            Connect();
        }
    }
    
    private void Connect()
    {
        _loginButtonText.text = "Connect...";
        _loginButtonText.color = Color.black;
        if (PhotonNetwork.IsConnected)
            return;

        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.GameVersion = Application.version;
    }

    public override void OnConnectedToMaster()
    {
        _isConnected = true;
        _loginButtonText.text = "Connected";
        _loginButtonText.color = Color.green;
        Debug.Log("Connected to Master Server.");
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _isConnected = false;
        _loginButtonText.text = "Disconnected";
        _loginButtonText.color = Color.red;
        Debug.Log("Disconnected. Check connect: " + PhotonNetwork.IsConnected);
    }

    private void OnDestroy()
    {
        //_loginButton.onClick.RemoveListener(LoginButton);
    }
}