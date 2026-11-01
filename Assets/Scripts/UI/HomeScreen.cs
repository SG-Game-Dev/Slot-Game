using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // Home scene controller: balance, Start Game, Settings, Exit.
    public class HomeScreen : MonoBehaviour
    {
        [SerializeField] private BalanceView balanceView;
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private SettingsPopup settingsPopup;
        [SerializeField] private ConfirmPopup confirmPopup;
        [SerializeField] private TMP_Text farewellText; // optional: shown in WebGL where the tab can't be closed

        private void Awake()
        {
            balanceView.Bind(GameServices.Wallet);
            settingsPopup.Bind(GameServices.Settings, GameServices.Progress);
            if (farewellText != null) farewellText.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartClicked);
            settingsButton.onClick.AddListener(settingsPopup.Open);
            exitButton.onClick.AddListener(OnExitClicked);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartClicked);
            settingsButton.onClick.RemoveListener(settingsPopup.Open);
            exitButton.onClick.RemoveListener(OnExitClicked);
        }

        private void OnStartClicked() => GameServices.SceneLoader.Load(SceneNames.Game);

        private void OnExitClicked() => confirmPopup.Show("Exit the game?", OnExitConfirmed);

        private void OnExitConfirmed()
        {
            bool closed = ApplicationExit.TryQuit();
            if (!closed && farewellText != null)
                farewellText.gameObject.SetActive(true);
        }
    }
}
