using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using System;

namespace G12 {
    public class PlayerNameInput : MonoBehaviour {

        [Header("UI")]
        [SerializeField] public TMP_InputField playerNameInput = null;
        [SerializeField] private Button continueButton = null;

        public static string DisplayName { get; private set; }

        private void Start() => SetUpInputField();

        private void SetUpInputField() {
            SetPlayerName(playerNameInput.text);
        }

        public void SetPlayerName(string name) => continueButton.interactable = !string.IsNullOrEmpty(name);

        public void SavePlayerName() {
            DisplayName = playerNameInput.text;
            
        }

    }
}

