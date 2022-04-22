using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using TMPro;

namespace G12 {
    public class Timer : NetworkBehaviour {

        [SyncVar] [SerializeField] public float startTime;
        [SyncVar] [SerializeField] public float currentTime = 0f;
        [SyncVar] [SerializeField] public int tiles = 4;
        [SyncVar] [SerializeField] public List<GameObject> all = new List<GameObject>();
        [SerializeField] public TMP_Text timerText;
        [SyncVar] [SerializeField] public GameObject gameMaster;
        [SyncVar] [SerializeField] public List<GameObject> players = new List<GameObject>();
        //[SyncVar] [SerializeField] GameObject tile;

        [SerializeField] NetworkManagerLobby lobby;

        public void Start() {

            lobby = (NetworkManagerLobby)FindObjectOfType(typeof(NetworkManagerLobby));

            currentTime = startTime;
            players = GameObject.FindGameObjectsWithTag("Player").ToList();
            foreach (GameObject player in players) {
                if (player.GetComponent<NetworkLobbyPlayer>().gamemaster == true) {
                    gameMaster = player;
                }
            }
            players.Remove(gameMaster);
            
        }


        public void Update() {          
            if (tiles == 1 || players.Count == 0) { // One tile left == Game Over
                timerText.text = "Game Over!";
            }

            else if (tiles != 1) {
                currentTime -= 1 * Time.deltaTime;
                timerText.text = currentTime.ToString("0");
            }

        }

    }
}

