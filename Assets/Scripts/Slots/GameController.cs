using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // Gameplay scene controller. It only coordinates: bet -> RNG -> animate -> show win -> pay.
    // Rules live in PayoutCalculator, money in Wallet, motion in Reel, drawing in the *View classes.
    public class GameController : MonoBehaviour
    {
        [Header("Slot Machine")]
        [SerializeField] private Reel[] reels;
        [SerializeField] private SlotHandle handle;

        [Header("HUD")]
        [SerializeField] private BalanceView balanceView;
        [SerializeField] private BetView betView;
        [SerializeField] private WinView winView;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button settingsButton;

        [Header("Popups")]
        [SerializeField] private PausePopup pausePopup;
        [SerializeField] private SettingsPopup settingsPopup;
        [SerializeField] private ConfirmPopup confirmPopup;

        [Header("Timing (seconds)")]
        [SerializeField] private float baseSpinTime = 1f;   // full-speed time of the first reel
        [SerializeField] private float stopStagger = 0.5f;  // extra time for each next reel
        [SerializeField] private float winShowTime = 1.2f;  // how long the win amount is shown before it is added

        private IWallet wallet;
        private IBetSelector betSelector;
        private IOutcomeGenerator outcomeGenerator;
        private IPayoutCalculator payoutCalculator;
        private ISceneLoader sceneLoader;

        private bool isSpinning;
        private bool isPaused;

        private void Awake()
        {
            wallet = GameServices.Wallet;
            betSelector = GameServices.BetSelector;
            sceneLoader = GameServices.SceneLoader;
            outcomeGenerator = new WeightedOutcomeGenerator(GameServices.Catalog);
            payoutCalculator = new PayoutCalculator(GameServices.Catalog);

            foreach (var reel in reels) reel.Initialize(GameServices.Catalog);

            balanceView.Bind(wallet);
            betView.Bind(betSelector);
            settingsPopup.Bind(GameServices.Settings, GameServices.Progress);
            winView.Clear();
        }

        private void OnEnable()
        {
            handle.Clicked += OnSpinRequested;
            pauseButton.onClick.AddListener(OpenPause);
            settingsButton.onClick.AddListener(settingsPopup.Open);
            pausePopup.ResumeRequested += ClosePause;
            pausePopup.HomeRequested += OnHomeRequested;
        }

        private void OnDisable()
        {
            handle.Clicked -= OnSpinRequested;
            pauseButton.onClick.RemoveListener(OpenPause);
            settingsButton.onClick.RemoveListener(settingsPopup.Open);
            pausePopup.ResumeRequested -= ClosePause;
            pausePopup.HomeRequested -= OnHomeRequested;
        }

        private void OnDestroy() => Time.timeScale = 1f; // safety: never leave the game frozen

        // ---------- Pause ----------
        private void OpenPause()
        {
            isPaused = true;
            Time.timeScale = 0f; // reels use Time.deltaTime / WaitForSeconds, so they freeze too
            pausePopup.SetHomeInteractable(!isSpinning); // leaving mid-spin would lose the prize
            pausePopup.Open();
        }

        private void ClosePause()
        {
            pausePopup.Close();
            Time.timeScale = 1f;
            isPaused = false;
        }

        private void OnHomeRequested()
        {
            confirmPopup.Show("Go back to home?", () => sceneLoader.Load(SceneNames.Home));
        }

        // ---------- Round ----------
        private void OnSpinRequested()
        {
            if (isSpinning || isPaused) return;

            int bet = betSelector.CurrentBet;
            if (!wallet.TrySpend(bet))
            {
                winView.ShowMessage("NOT ENOUGH BALANCE");
                return;
            }
            StartCoroutine(PlayRound(bet));
        }

        private IEnumerator PlayRound(int bet)
        {
            isSpinning = true;
            SetControlsInteractable(false);
            winView.Clear();

            // 1) RNG decides the result first.
            var result = reels.Select(_ => outcomeGenerator.Next()).ToList();

            // 2) Pull the lever, then reels only animate towards that result.
            yield return handle.PlayPull();
            for (int i = 0; i < reels.Length; i++)
                reels[i].Spin(result[i], baseSpinTime + i * stopStagger);

            yield return new WaitUntil(() => reels.All(r => r.IsStopped));

            // 3) Show the win amount, THEN add it to the balance.
            int prize = payoutCalculator.Calculate(result, bet);
            if (prize > 0)
            {
                winView.Show(prize);
                yield return new WaitForSeconds(winShowTime);
                wallet.Add(prize);
            }
            else
            {
                winView.ShowMessage("TRY AGAIN");
            }

            isSpinning = false;
            SetControlsInteractable(true);
        }

        private void SetControlsInteractable(bool value)
        {
            betView.SetInteractable(value);
            settingsButton.interactable = value;
            handle.SetInteractable(value);
        }
    }
}
