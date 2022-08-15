using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionController : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
        [SerializeField] private ServerSettings serverSettings;
        [SerializeField] private GameObject roomListHolder;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Button createRoom;
        [SerializeField] private Button closeRoom;
        [SerializeField] private Button createRoomForFiend;
        [SerializeField] private GameObject prefabTMP;
        [SerializeField] private Button joinRandomRoom;
        [SerializeField] private Button createLevelFilterRoom;
        [SerializeField] private Button defaultLobby;
        [SerializeField] private Button customLobby;
        private LoadBalancingClient _loadBalancingClient;
        private readonly List<GameObject> _createdRooms = new ();
        private const string LevelFilterKey = "C0";

        private void Start()
        {
            _loadBalancingClient = new LoadBalancingClient();
            _loadBalancingClient.AddCallbackTarget(this);
            createRoom.onClick.AddListener(OnCreateRoom);
            closeRoom.onClick.AddListener(OnCloseRoom);
            createRoomForFiend.onClick.AddListener(OnCreateRoomForFriends);
            joinRandomRoom.onClick.AddListener(OnJoinRandomRoom);
            createLevelFilterRoom.onClick.AddListener(OnCreateLevelFilterRoom);
            defaultLobby.onClick.AddListener(OnDefaultLobby);
            customLobby.onClick.AddListener(OnCustomLobby);
            createRoom.gameObject.SetActive(false);
            closeRoom.gameObject.SetActive(false);
            createRoomForFiend.gameObject.SetActive(false);
            joinRandomRoom.gameObject.SetActive(false);
            createLevelFilterRoom.gameObject.SetActive(false);
            defaultLobby.gameObject.SetActive(false);
            customLobby.gameObject.SetActive(false);

            _loadBalancingClient.ConnectUsingSettings(serverSettings.AppSettings);
        }

        private void OnDestroy()
        {
            _loadBalancingClient.RemoveCallbackTarget(this);
            createRoom.onClick.RemoveListener(OnCreateRoom);
            closeRoom.onClick.RemoveListener(OnCloseRoom);
            createRoomForFiend.onClick.RemoveListener(OnCreateRoomForFriends);
            joinRandomRoom.onClick.RemoveListener(OnJoinRandomRoom);
            createLevelFilterRoom.onClick.RemoveListener(OnCreateLevelFilterRoom);
            defaultLobby.onClick.RemoveListener(OnDefaultLobby);
            customLobby.onClick.RemoveListener(OnCustomLobby);
        }

        private void Update()
        {
            _loadBalancingClient?.Service();
        }

        private void OnCreateRoom()
        {
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 4
            };
            var enterRoomParams = new EnterRoomParams
            {
                RoomName = "Test Room",
                RoomOptions = roomOptions
            };
            _loadBalancingClient.OpCreateRoom(enterRoomParams);
        }
        
        private void OnCloseRoom()
        {
            _loadBalancingClient.CurrentRoom.IsOpen = false;
            closeRoom.gameObject.SetActive(false);
            Debug.Log($"Room is open: {_loadBalancingClient.CurrentRoom.IsOpen}");
        }
        
        private void OnCreateRoomForFriends()
        {
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 4,
                IsVisible = false
            };
            var enterRoomParams = new EnterRoomParams
            {
                RoomName = Guid.NewGuid().ToString(),
                RoomOptions = roomOptions
            };
            _loadBalancingClient.OpCreateRoom(enterRoomParams);
        }
        
        private void OnJoinRandomRoom()
        {
            var sqlLobbyFilter = "C0 BETWEEN 1 and 5";
            var joinRandomRoomPrams = new OpJoinRandomRoomParams
            {
                SqlLobbyFilter = sqlLobbyFilter
            };
            _loadBalancingClient.OpJoinRandomRoom(joinRandomRoomPrams);
        }
        
        private void OnCreateLevelFilterRoom()
        {
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 4,
                CustomRoomProperties = new Hashtable
                {
                    {LevelFilterKey, 1}
                },
                CustomRoomPropertiesForLobby = new []
                {
                    LevelFilterKey
                }
            };
            var enterRoomParams = new EnterRoomParams
            {
                RoomName = Guid.NewGuid().ToString(),
                RoomOptions = roomOptions,
                Lobby = new TypedLobby("CustomSqlLobby", LobbyType.SqlLobby)
            };
            _loadBalancingClient.OpCreateRoom(enterRoomParams);
        }
        
        private void OnCustomLobby()
        {
            _loadBalancingClient.OpJoinLobby(new TypedLobby("CustomSqlLobby", LobbyType.SqlLobby));
        }

        private void OnDefaultLobby()
        {
            _loadBalancingClient.OpJoinLobby(TypedLobby.Default);
        }
        
        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        { 
            defaultLobby.gameObject.SetActive(true);
            customLobby.gameObject.SetActive(true);
            Debug.Log("OnConnectedToMaster");
        }

        public void OnDisconnected(DisconnectCause cause)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");
            createRoom.gameObject.SetActive(false);
            createRoomForFiend.gameObject.SetActive(false);
            createLevelFilterRoom.gameObject.SetActive(false);
            closeRoom.gameObject.SetActive(true);
            if (!_loadBalancingClient.CurrentRoom.IsVisible)
            {
                var text = Instantiate(prefabTMP, roomListHolder.transform);
                text.GetComponent<TMP_Text>().text = _loadBalancingClient.CurrentRoom.Name;
            }
            Debug.Log($"Room is open: {_loadBalancingClient.CurrentRoom.IsOpen}");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            joinRandomRoom.gameObject.SetActive(false);
            Debug.Log("OnJoinedRoom");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(message);
        }

        public void OnLeftRoom()
        {
        }

        public void OnJoinedLobby()
        {
            if (_loadBalancingClient.CurrentLobby.Type == LobbyType.SqlLobby)
            {
                joinRandomRoom.gameObject.SetActive(true);
                createLevelFilterRoom.gameObject.SetActive(true);
            }
            else
            {
                createRoom.gameObject.SetActive(true); 
                createRoomForFiend.gameObject.SetActive(true); 
            }
            defaultLobby.gameObject.SetActive(false);
            customLobby.gameObject.SetActive(false);
            Debug.Log("OnJoinedLobby"); 
        }

        public void OnLeftLobby()
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (var createdRoom in _createdRooms)
            {
                Destroy(createdRoom);
            }
            _createdRooms.Clear();
            for (int i = 0; i < roomList.Count; i++)
            {
                var button = Instantiate(buttonPrefab, roomListHolder.transform);
                button.GetComponentInChildren<TMP_Text>().text = roomList[i].Name;
                _createdRooms.Add(button);
            }
            Debug.Log($"OnRoomListUpdate. Room count: {roomList.Count}");
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }
}