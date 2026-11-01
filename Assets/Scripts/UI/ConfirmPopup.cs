using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // Reusable Yes / No dialog: confirmPopup.Show("Exit the game?", () => ...);
    public class ConfirmPopup : Popup
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private Action onYes;

        protected override void Awake()
        {
            base.Awake();
            yesButton.onClick.AddListener(OnYesClicked);
            noButton.onClick.AddListener(Close);
        }

        public void Show(string message, Action onYesCallback)
        {
            messageText.text = message;
            onYes = onYesCallback;
            Open();
        }

        private void OnYesClicked()
        {
            Close();
            var callback = onYes;
            onYes = null;
            callback?.Invoke();
        }
    }
}
