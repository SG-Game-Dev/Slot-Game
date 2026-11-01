using System;

namespace UnderpinSlots
{
    // Responsible only for the player's money. No UI, no saving, no Unity types.
    public class Wallet : IWallet
    {
        public int Balance { get; private set; }
        public event Action<int> BalanceChanged = delegate { };

        public Wallet(int startingBalance)
        {
            Balance = Math.Max(0, startingBalance);
        }

        public bool CanAfford(int amount) => amount >= 0 && Balance >= amount;

        public bool TrySpend(int amount)
        {
            if (!CanAfford(amount)) return false;
            Balance -= amount;
            BalanceChanged(Balance);
            return true;
        }

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Balance += amount;
            BalanceChanged(Balance);
        }

        public void Reset(int balance)
        {
            Balance = Math.Max(0, balance);
            BalanceChanged(Balance);
        }
    }
}
