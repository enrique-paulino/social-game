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
        public GameObject[] currentTiles;
        [SerializeField] private LayerMask playerMask;

        [Header("Lobby")]
        [SerializeField] private NetworkLobbyPlayer lobbyPlayerPrefab = null;
        [SerializeField] private MainMenu menuScript = null;
        [SerializeField] public NetworkLobbyPlayer gameMasterObject;

        [Header("GameMaster")]
        public int gamemaster;
        public Color blue;
        public Color red;

        public List<GameObject> tiles;

        public string playerName;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        public List<NetworkLobbyPlayer> LobbyPlayers { get; } = new List<NetworkLobbyPlayer>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        private void Update() {
            menuScript = MainMenu.FindObjectOfType<MainMenu>();
            if (SceneManager.GetActiveScene().buildIndex == 1) {
                FindPlayerPos();
                DeleteTiles();
                
            }

            tiles = GameObject.FindGameObjectsWithTag("Tile").ToList();
        }

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
            //menuScript.EnableUI();
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
                currentTiles = new GameObject[LobbyPlayers.Count];
                RandomPlayer();
                ChangeColour();
                ResetPosition();
                ServerChangeScene("Minigame_Falling_Tiles"); // Temporary. Change to some logic to choose a random mini game

            }
        }

        public override void ServerChangeScene(string newSceneName) {

            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Minigame")) {
                for (int i = LobbyPlayers.Count - 1; i >= 0; i--) {
                    var conn = LobbyPlayers[i];
                                       
                    NetworkManager.DontDestroyOnLoad(conn); // Player still exists when the server changes                    
                }
            }

            base.ServerChangeScene(newSceneName);
        }

        public void RandomPlayer() {
            //gameMasterObject = LobbyPlayers[UnityEngine.Random.Range(0, LobbyPlayers.Count)];
            gameMasterObject = LobbyPlayers[0];
            foreach (var player in LobbyPlayers) {
                gameMasterObject.gamemaster = true;
                
            }

        }

        public void ResetPosition() {
            foreach (NetworkLobbyPlayer player in LobbyPlayers) {
                player.transform.position = Vector3.zero;
            }
        }

        // For falling tiles minigame
        public void ChangeColour() {
            foreach (NetworkLobbyPlayer player in LobbyPlayers) {
                if (player == gameMasterObject) {
                    player.GetComponent<FallingTilePlayerController>().enabled = true;
                }
            }
        }

        public void FindPlayerPos() {
            for (int i = 0; i < LobbyPlayers.Count; i++) {
                foreach (GameObject eachTile in tiles) {
                    try {
                        if (eachTile.GetComponent<BoxCollider2D>().bounds.Contains(LobbyPlayers[i].transform.position)) {
                            currentTiles[i] = eachTile;
                            if (!tiles.Contains(currentTiles[i])) {
                                currentTiles[i] = null;
                            }
                        }
                    }
                    catch (Exception e) {
                        print(e);
                    }
                    
                }
                
            }
        }



        public void DeleteTiles() {
            GameObject tile = currentTiles[gameMasterObject.GetComponent<NetworkLobbyPlayer>().netId - 1];

            Timer timer = (Timer)FindObjectOfType(typeof(Timer));

            if (!timer.all.Contains(tile)) {
                GameObject[] tiles2 = GameObject.FindGameObjectsWithTag("Tile");
                int index = UnityEngine.Random.Range(0, tiles2.Length);
                tile = tiles2[index];
            }

            if (timer.currentTime <= 0) {

                for (int i = 0; i < LobbyPlayers.Count; i++) {
                    if (currentTiles[i] == tile || currentTiles[i] == null) {
                        if (LobbyPlayers[i].gamemaster == true) { continue; }
                        foreach (NetworkLobbyPlayer player in LobbyPlayers) {
                            player.KillPlayer(LobbyPlayers[i], timer);
                        }
                        
                    }
                }

                tiles.Remove(tile);
                timer.all.Remove(tile);
                NetworkServer.Destroy(tile);




                timer.currentTime = timer.startTime;
                timer.tiles = timer.tiles - 1;
                tile = null;
            }


        }

        


    }
}

