using System;

namespace UnderpinSlots
{
    public interface IBetSelector
    {
        int CurrentBet { get; }
        bool CanIncrease { get; }
        bool CanDecrease { get; }
        event Action<int> BetChanged;

        void Increase();
        void Decrease();
        void Reset();
    }
}
