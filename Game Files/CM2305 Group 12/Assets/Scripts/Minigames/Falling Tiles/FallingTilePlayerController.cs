using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

namespace G12 {
    public class FallingTilePlayerController : NetworkBehaviour {

        public NetworkManagerLobby lobby;
        public GameObject gmText;

        public void Start() {
            lobby = (NetworkManagerLobby)FindObjectOfType(typeof(NetworkManagerLobby));
            StartCoroutine(ShowRole(3));
        }


        public void Update() {
            if (!this.GetComponent<NetworkLobbyPlayer>().gamemaster) { return; }
            if (!hasAuthority) { return; }
            int gm = (int)GetComponent<NetworkLobbyPlayer>().netId - 1;
            GameObject tile = lobby.currentTiles[gm];
            
            List<GameObject> tiles = GameObject.FindGameObjectsWithTag("Tile").ToList();
            foreach (GameObject single in tiles) {               
                single.GetComponent<SpriteRenderer>().color = lobby.blue;
            }
            tile.GetComponent<SpriteRenderer>().color = lobby.red;

        }

        IEnumerator ShowRole(float seconds) {
            gmText.SetActive(true);
            yield return new WaitForSeconds(seconds);
            gmText.SetActive(false);
        }
    }
}



