using UnityEngine;

namespace UnderpinSlots
{
    // Composition root: builds every service ONCE, before the first scene loads, so
    // Home and Game scenes share the same wallet / bet / settings.
    // Everything is exposed through interfaces (Dependency Inversion).
    public static class GameServices
    {
        public static IWallet Wallet { get; private set; }
        public static IBetSelector BetSelector { get; private set; }
        public static ISettingsService Settings { get; private set; }
        public static IProgressResetter Progress { get; private set; }
        public static ISymbolCatalog Catalog { get; private set; }
        public static ISceneLoader SceneLoader { get; private set; }

        // Kept in a field so it is not garbage collected (it listens to service events).
        private static PlayerDataSaver saver;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            var config = Resources.Load<SlotConfig>("SlotConfig");
            if (config == null)
            {
                Debug.LogError("SlotConfig asset not found. Create it at Assets/Resources/SlotConfig.asset");
                return;
            }
            Initialize(config, new PlayerPrefsDataRepository());
        }

        // Public so tests can inject a fake repository.
        public static void Initialize(SlotConfig config, IPlayerDataRepository repository)
        {
            if (!repository.TryLoad(out PlayerData data))
                data = PlayerData.CreateDefault(config.StartingBalance, config.BetOptions[0]);

            Catalog = config;
            Wallet = new Wallet(data.balance);
            BetSelector = new BetSelector(config.BetOptions, data.bet);
            Settings = new SettingsService(data.soundOn);
            Progress = new ProgressResetter(Wallet, BetSelector, config.StartingBalance);
            SceneLoader = new SceneLoader();
            saver = new PlayerDataSaver(repository, Wallet, BetSelector, Settings);
        }
    }
}
