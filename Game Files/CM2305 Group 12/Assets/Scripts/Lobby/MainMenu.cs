using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G12 {
    public class MainMenu : MonoBehaviour {

        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] public GameObject landingPagePanel = null;
        [SerializeField] public GameObject gameTitle = null;
        [SerializeField] public GameObject environment = null;

        public void HostLobby() {
            networkManager.StartHost();
            landingPagePanel.SetActive(false);
            gameTitle.SetActive(false);
            environment.SetActive(true);
        }

        public void EnableUI() {
            landingPagePanel.SetActive(true);
            gameTitle.SetActive(true);
            environment.SetActive(false);
        }

        public void EnableMap() {
            environment.SetActive(true);
        }

    }
}

