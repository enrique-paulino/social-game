using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;

namespace G12 {
    public class NetworkManagerLobby : NetworkManager {

        [SerializeField] private int minPlayers = 2;
        [Scene] [SerializeField] private string menuScene = string.Empty;

        [Header("Lobby")]
        [SerializeField] private NetworkLobbyPlayer lobbyPlayerPrefab = null;
        [SerializeField] private MainMenu menuScript = null;

        public string playerName;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        public List<NetworkLobbyPlayer> LobbyPlayers { get; } = new List<NetworkLobbyPlayer>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        public override void OnStartClient() {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs) {
                NetworkClient.RegisterPrefab(prefab);
                playerName = null;
            }
        }

        public override void OnClientConnect() {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
            menuScript.EnableMap();
        }

        public override void OnClientDisconnect() {
            menuScript.EnableUI();
            base.OnClientDisconnect();
            OnClientDisconnected?.Invoke();
            
        }

        public override void OnServerConnect(NetworkConnection conn) {
            if (numPlayers >= maxConnections) {
                conn.Disconnect();
                return;
            }

            if (SceneManager.GetActiveScene().path != menuScene) {
                conn.Disconnect();
                return;
            }

            foreach (var player in LobbyPlayers) {
                player.HandleReadyToStart(false);
            }


        }

        public override void OnServerAddPlayer(NetworkConnection conn) {
            if (SceneManager.GetActiveScene().path == menuScene) {

                bool isLeader = LobbyPlayers.Count == 0;

                NetworkLobbyPlayer playerInstance = Instantiate(lobbyPlayerPrefab);

                playerInstance.IsLeader = isLeader;

                NetworkServer.AddPlayerForConnection(conn, playerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn) {

            if (conn.identity != null) {
                var player = conn.identity.GetComponent<NetworkLobbyPlayer>();
                LobbyPlayers.Remove(player);


                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);

        }

        public override void OnStopServer() {
            LobbyPlayers.Clear();
        }

        public void NotifyPlayersOfReadyState() {
            foreach (var player in LobbyPlayers) {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        private bool IsReadyToStart() {
            if (numPlayers < minPlayers) { return false; }

            foreach (var player in LobbyPlayers) {
                if (!player.IsReady) { return false; }
            }

            return true;
        }

        public void StartGame() {
            if (SceneManager.GetActiveScene().path == menuScene) {
                if (!IsReadyToStart()) { return; }
                ServerChangeScene("Scene_Minigame_01"); // Temporary. Change to some logic to choose a random mini game
            }
        }

        public override void ServerChangeScene(string newSceneName) {

            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Scene_Minigame")) {

                for (int i = LobbyPlayers.Count - 1; i >= 0; i--) {
                    var conn = LobbyPlayers[i];
                    NetworkManager.DontDestroyOnLoad(conn); // Player still exists when the server changes
                    NetworkManager.DontDestroyOnLoad(Camera.main); // Keeps camera attached to player (no duplicates)
                }

            }

            base.ServerChangeScene(newSceneName);
        }



    }
}

