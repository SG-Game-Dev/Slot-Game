namespace UnderpinSlots
{
    // Observer: saves automatically whenever balance, bet or settings change.
    // Game code never calls "Save" by hand, so nobody can forget it.
    public class PlayerDataSaver
    {
        private readonly IPlayerDataRepository repository;
        private readonly IWallet wallet;
        private readonly IBetSelector betSelector;
        private readonly ISettingsService settings;

        public PlayerDataSaver(IPlayerDataRepository repository, IWallet wallet,
                               IBetSelector betSelector, ISettingsService settings)
        {
            this.repository = repository;
            this.wallet = wallet;
            this.betSelector = betSelector;
            this.settings = settings;

            wallet.BalanceChanged += _ => Save();
            betSelector.BetChanged += _ => Save();
            settings.SoundChanged += _ => Save();
        }

        public void Save()
        {
            repository.Save(new PlayerData
            {
                balance = wallet.Balance,
                bet = betSelector.CurrentBet,
                soundOn = settings.SoundOn
            });
        }
    }
}
