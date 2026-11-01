using System.Collections.Generic;

namespace UnderpinSlots
{
    // Win rule from the assignment: the player wins only when ALL symbols are the same.
    // Prize = bet x multiplier of that symbol (multipliers live in SlotConfig, not in code).
    public class PayoutCalculator : IPayoutCalculator
    {
        private readonly ISymbolCatalog catalog;

        public PayoutCalculator(ISymbolCatalog catalog)
        {
            this.catalog = catalog;
        }

        public int Calculate(IReadOnlyList<SymbolType> payLine, int bet)
        {
            if (payLine == null || payLine.Count == 0) return 0;

            SymbolType first = payLine[0];
            for (int i = 1; i < payLine.Count; i++)
                if (payLine[i] != first) return 0;

            return catalog.Get(first).winMultiplier * bet;
        }
    }
}
