using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace G12
{


    public class Timer : MonoBehaviour
    {

        public float timeValue = 60f;
        public Text timerText;
        private NetworkManagerLobby lobby;
        public NetworkManagerLobby Lobby
        {
            get
            {
                if (lobby != null) { return lobby; }
                return lobby = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Lobby.LobbyPlayers.Count > 0)
            {
                if (timeValue > 0)
                {
                    timeValue -= Time.deltaTime;
                }
                else
                {
                    timeValue = 0;
                }
                displayTime(timeValue);
            }
        }

        IEnumerator Order()
        {
            Transform timeUp = transform.Find("TimeUp");
            timeUp.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            timeUp.gameObject.SetActive(false);
            NetworkManager.singleton.StopClient();
            NetworkManager.singleton.StopHost();
        }
        void displayTime(float timeToDisplay)
        {
            if (timeToDisplay < 0)
            {
                timeToDisplay = 0;
                StartCoroutine(Order());
            }

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            this.GetComponent<Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
