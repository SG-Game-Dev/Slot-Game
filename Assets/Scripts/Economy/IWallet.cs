using System;

namespace UnderpinSlots
{
    public interface IWallet
    {
        int Balance { get; }
        event Action<int> BalanceChanged;

        bool CanAfford(int amount);
        bool TrySpend(int amount);
        void Add(int amount);
        void Reset(int balance);
    }
}
