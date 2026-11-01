using System.Collections.Generic;

namespace UnderpinSlots
{
    public interface IPayoutCalculator
    {
        // Returns the prize for the symbols on the pay line (0 = no win).
        int Calculate(IReadOnlyList<SymbolType> payLine, int bet);
    }
}
