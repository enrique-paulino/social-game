using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace G12 {
    public class JoinLobbyMenu : MonoBehaviour {

        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private GameObject gameTitle = null;
        [SerializeField] private TMP_InputField ipAddressInput = null;
        [SerializeField] private Button joinButton = null;

        private void Start() {
            networkManager = FindObjectOfType<NetworkManagerLobby>();
        }

        private void OnEnable() {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientConnected += HandleClientDisconnected;
        }

        private void OnDisable() {
            NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
            NetworkManagerLobby.OnClientConnected -= HandleClientDisconnected;
        }

        private void HandleClientConnected() {
            joinButton.interactable = true;
            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
            gameTitle.SetActive(false);
        }

        private void HandleClientDisconnected() {
            joinButton.interactable = true;
        }

        public void JoinLobby() {
            string ipAddress = ipAddressInput.text;

            networkManager.networkAddress = ipAddress;
            networkManager.StartClient();

            joinButton.interactable = false;
        }

    }
}

