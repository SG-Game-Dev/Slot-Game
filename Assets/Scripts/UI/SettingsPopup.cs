using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    public class SettingsPopup : Popup
    {
        [SerializeField] private Button soundButton;
        [SerializeField] private TMP_Text soundLabel;
        [SerializeField] private Button resetButton;

        private ISettingsService settings;
        private IProgressResetter progress;

        // Dependencies are handed in by the screen that owns this popup.
        public void Bind(ISettingsService settingsService, IProgressResetter progressResetter)
        {
            settings = settingsService;
            progress = progressResetter;
        }

        protected override void Awake()
        {
            base.Awake();
            soundButton.onClick.AddListener(ToggleSound);
            resetButton.onClick.AddListener(() => progress.ResetProgress());
        }

        public override void Open()
        {
            base.Open();
            RefreshSoundLabel();
        }

        private void ToggleSound()
        {
            settings.SetSound(!settings.SoundOn);
            RefreshSoundLabel();
        }

        private void RefreshSoundLabel() => soundLabel.text = "SOUND: " + (settings.SoundOn ? "ON" : "OFF");
    }
}
