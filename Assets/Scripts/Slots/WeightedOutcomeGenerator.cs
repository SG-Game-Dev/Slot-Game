using System;
using System.Collections.Generic;

namespace UnderpinSlots
{
    // Weighted RNG. The result is decided BEFORE the reels spin; the reels only animate towards it.
    public class WeightedOutcomeGenerator : IOutcomeGenerator
    {
        private readonly IReadOnlyList<SymbolDefinition> symbols;
        private readonly Random rng;
        private readonly int totalWeight;

        // Pass a seeded System.Random for reproducible tests.
        public WeightedOutcomeGenerator(ISymbolCatalog catalog, Random rng = null)
        {
            symbols = catalog.Symbols;
            this.rng = rng ?? new Random();

            foreach (var s in symbols) totalWeight += s.weight;
        }

        public SymbolType Next()
        {
            if (totalWeight <= 0) return symbols[0].symbol;

            // Roll in [0, totalWeight) and walk the cumulative weights.
            int roll = rng.Next(totalWeight);
            foreach (var s in symbols)
            {
                if (roll < s.weight) return s.symbol;
                roll -= s.weight;
            }
            return symbols[0].symbol; // unreachable
        }
    }
}
