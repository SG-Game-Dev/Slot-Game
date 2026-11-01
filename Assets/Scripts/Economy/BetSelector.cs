using System;
using System.Collections.Generic;

namespace UnderpinSlots
{
    // Steps through a fixed list of allowed bets (e.g. 10, 50, 100, 500).
    public class BetSelector : IBetSelector
    {
        private readonly IReadOnlyList<int> options;
        private int index;

        public event Action<int> BetChanged = delegate { };

        public int CurrentBet => options[index];
        public bool CanIncrease => index < options.Count - 1;
        public bool CanDecrease => index > 0;

        public BetSelector(IReadOnlyList<int> options, int initialBet)
        {
            if (options == null || options.Count == 0)
                throw new ArgumentException("At least one bet option is required.", nameof(options));

            this.options = options;

            // Restore the saved bet; fall back to the lowest option if it no longer exists.
            index = 0;
            for (int i = 0; i < options.Count; i++)
                if (options[i] == initialBet) index = i;
        }

        public void Increase()
        {
            if (!CanIncrease) return;
            index++;
            BetChanged(CurrentBet);
        }

        public void Decrease()
        {
            if (!CanDecrease) return;
            index--;
            BetChanged(CurrentBet);
        }

        public void Reset()
        {
            index = 0;
            BetChanged(CurrentBet);
        }
    }
}
