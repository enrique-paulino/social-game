using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

namespace G12 {


    public class FallingTileController : NetworkBehaviour {

        [SerializeField] public GameObject tile = null;

        public void OnTriggerStay2D(Collider2D collision) {
            if (GetComponent<NetworkLobbyPlayer>().gamemaster == false) return;
            if (collision.tag != "Tile") { return; }

            tile = collision.gameObject;
        }

        

        public void OnTriggerEnter2D(Collider2D collision) {
            if (GetComponent<NetworkLobbyPlayer>().gamemaster == false) return;
            if (collision.tag != "Tile") { return; }

            tile = collision.gameObject;

        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (GetComponent<NetworkLobbyPlayer>().gamemaster == false) return;
            if (collision.tag != "Tile") { return; }

            tile = null;

        }

    }
}

