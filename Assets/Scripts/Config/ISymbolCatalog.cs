using System.Collections.Generic;

namespace UnderpinSlots
{
    // Read-only view of the symbol data. Reels, RNG and payout depend on THIS, not on the ScriptableObject.
    public interface ISymbolCatalog
    {
        // The order of this list is the order of symbols on the reel strip.
        IReadOnlyList<SymbolDefinition> Symbols { get; }
        SymbolDefinition Get(SymbolType type);
    }
}
