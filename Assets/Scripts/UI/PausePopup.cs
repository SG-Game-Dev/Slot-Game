using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // Only raises events; it does not know what "resume" or "home" mean (the controller decides).
    public class PausePopup : Popup
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;

        public event Action ResumeRequested = delegate { };
        public event Action HomeRequested = delegate { };

        protected override void Awake()
        {
            base.Awake();
            resumeButton.onClick.AddListener(() => ResumeRequested());
            homeButton.onClick.AddListener(() => HomeRequested());
        }

        public void SetHomeInteractable(bool value) => homeButton.interactable = value;
    }
}
