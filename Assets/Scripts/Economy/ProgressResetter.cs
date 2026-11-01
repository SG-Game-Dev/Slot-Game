namespace UnderpinSlots
{
    // Restores starting balance and lowest bet (used by Settings > Reset).
    public class ProgressResetter : IProgressResetter
    {
        private readonly IWallet wallet;
        private readonly IBetSelector betSelector;
        private readonly int startingBalance;

        public ProgressResetter(IWallet wallet, IBetSelector betSelector, int startingBalance)
        {
            this.wallet = wallet;
            this.betSelector = betSelector;
            this.startingBalance = startingBalance;
        }

        public void ResetProgress()
        {
            wallet.Reset(startingBalance);
            betSelector.Reset();
        }
    }
}
