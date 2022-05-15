using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using UnityEngine.SceneManagement;

namespace G12 {
    public class NetworkLobbyPlayer : NetworkBehaviour {

        [Header("Movement")]
        private Vector2 movement;
        private Camera cam;
        [SerializeField] private Rigidbody2D rb;     
        [SerializeField] private int speed;

        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string currentAnim;
        [SerializeField] private string[] animationNames = new string[5];

        [Header("UI")]
        [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] public TMP_Text playerName = null;
        [SerializeField] private string[] playerNameTexts = new string[10];
        [SerializeField] private Button startGameButton = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))] public string DisplayName = null;
        [SyncVar(hook = nameof(HandleReadyStatusChanged))] public bool IsReady = false;

        [SerializeField] public float points = 0;

        private bool isLeader;
        public bool IsLeader {
            set {
                isLeader = value;
                startGameButton.gameObject.SetActive(value);
            }
        }

        void Start()
        {
            Scene currentScene = SceneManager.GetActiveScene();
        }

        private NetworkManagerLobby lobby;
        private NetworkManagerLobby Lobby {
            get {
                if (lobby != null) { return lobby; }
                return lobby = NetworkManager.singleton as NetworkManagerLobby;
            }
        }
        
        public override void OnStartLocalPlayer() {                     
            if (!hasAuthority) { return; }
            cam = Camera.main;

        }


        public override void OnStartAuthority() {
            CmdSetDisplayName(PlayerNameInput.DisplayName); // potential to add logic (e.g. length of name, swearing)
            lobbyUI.SetActive(true);
        }

        public override void OnStartClient() {
            Lobby.LobbyPlayers.Add(this);

            UpdateDisplay();
        }

        public void OnNetworkDestroy() {
            Lobby.LobbyPlayers.Remove(this);
            UpdateDisplay();
        }

        public override void OnStopClient() {
            if (!hasAuthority) { return; }
            NetworkManager.singleton.StopClient();
            NetworkManager.singleton.StopHost();
            base.OnStopClient();
        }

        public void OnServerConnect() {
            Debug.Log("CONNECTED");

            foreach (var player in Lobby.LobbyPlayers) {
                player.UpdateDisplay();
            }
        }

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
        private void UpdateDisplay() {

            foreach (var player in Lobby.LobbyPlayers) {
                for (int i = 0; i < Lobby.LobbyPlayers.Count; i++) {
                    player.playerNameTexts[i] = Lobby.LobbyPlayers[i].DisplayName;
                    Lobby.LobbyPlayers[i].playerName.text = player.playerNameTexts[i];
                    if (player.IsReady) { player.playerName.color = Color.green; }
                    else { player.playerName.color = Color.red; }
                }
            }
            
        }

        public void HandleReadyToStart(bool readyToStart) {
            if (!isLeader) { return; }
            startGameButton.interactable = readyToStart;
        }

        [Command]
        private void CmdSetDisplayName(string displayName) {
            DisplayName = displayName;
        }

        [Command]
        public void CmdReadyUp() {
            IsReady = !IsReady;
            //if (this.IsReady) { player.playerName.color = Color.green; }
            Lobby.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame() {
            if (Lobby.LobbyPlayers[0].connectionToClient != connectionToClient) { return; }
            // Start Game
            Lobby.StartGame();
        }

        // Movement Start
        private void Update() {            
            if (!hasAuthority) { return; }
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            Scene currentScene = SceneManager.GetActiveScene();
            if (transform.position.x > -3.317157 && transform.position.x < 6.244987 && transform.position.y < 1.477157 && transform.position.y > -6.279886)
            {
                this.points += 1f*Time.deltaTime;
            }
            PlayAnimation();
        }

        private void FixedUpdate() {
            if (!hasAuthority) { return; }
            rb.velocity = movement.normalized * speed;
        }

        private void LateUpdate() {
            if (!hasAuthority) { return; }
            cam.transform.position = transform.position + 10 * Vector3.back; // Camera follow on player
        }

        void PlayAnimation() {
            if (movement == Vector2.zero) {
                ChangeAnimation(animationNames[0]);
            }
            else if (movement.x > 0) {
                ChangeAnimation(animationNames[1]);
            }
            else if (movement.x < 0) {
                ChangeAnimation(animationNames[2]);
            }
            else if (movement.y > 0) {
                ChangeAnimation(animationNames[3]);
            }
            else if (movement.y < 0) {
                ChangeAnimation(animationNames[4]);
            }
        }

        void ChangeAnimation(string newAnim) {
            if (newAnim == currentAnim) return;

            animator.Play(newAnim);
            currentAnim = newAnim;
        }

    }
}

