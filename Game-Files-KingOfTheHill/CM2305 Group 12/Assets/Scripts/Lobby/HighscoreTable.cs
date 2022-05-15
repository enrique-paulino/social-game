using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


namespace G12
{
    public class HighscoreTable : MonoBehaviour
    {

        private Transform entryContainer;
        private Transform entryTemplate;
        private List<HighscoreEntry> highscoreEntryList;
        private List<Transform> highscoreEntryTransformList;
        private NetworkManagerLobby lobby;
        public NetworkManagerLobby Lobby
        {
            get
            {
                if (lobby != null) { return lobby; }
                return lobby = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        private void Awake()
        {
            entryContainer = transform.Find("HighscoreEntryContainer");
            entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

            entryTemplate.gameObject.SetActive(false);
            highscoreEntryList = new List<HighscoreEntry>();
            for(int i = 0; i < 2; i++)
            {
                highscoreEntryList.Add(new HighscoreEntry { score = Random.Range(0, 100), name = "Player" });
            }
            if(lobby != null)
            {
                foreach (NetworkLobbyPlayer player in Lobby.LobbyPlayers)
                {
                highscoreEntryList.Add(new HighscoreEntry { score = (int)player.points, name = player.playerName.ToString() });
                }
            }


                //Bubble Sort
                for (int i = 0; i < highscoreEntryList.Count; i++)
            {
                for (int j = i + 1; j < highscoreEntryList.Count; j++)
                {
                    if (highscoreEntryList[j].score > highscoreEntryList[i].score)
                    {
                        HighscoreEntry tmp = highscoreEntryList[i];
                        highscoreEntryList[i] = highscoreEntryList[j];
                        highscoreEntryList[j] = tmp;
                    }
                }
            }
            highscoreEntryTransformList = new List<Transform>();
            foreach(HighscoreEntry highscoreEntry in highscoreEntryList)
            {
                CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }
        private void CreateHighScoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
        {
            float templateHeight = 30f;
                Transform entryTransform = Instantiate(entryTemplate, container);
                RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
                entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
                entryTransform.gameObject.SetActive(true);

                int rank = transformList.Count + 1;
                string rankString;
                switch (rank)
                {
                    default:
                        rankString = rank + "TH"; break;
                    case 1: rankString = "1ST"; break;
                    case 2: rankString = "2ND"; break;
                    case 3: rankString = "3RD"; break;

                }

                entryTransform.Find("posText").GetComponent<Text>().text = rankString;
                entryTransform.Find("nameText").GetComponent<Text>().text = highscoreEntry.score.ToString();
                entryTransform.Find("scoreText").GetComponent<Text>().text = highscoreEntry.name;

            transformList.Add(entryTransform);
        }
    }

    public class HighscoreEntry
    {
        public int score;
        public string name;
    }
}

