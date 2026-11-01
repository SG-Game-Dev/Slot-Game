using System.Collections.Generic;
using UnityEngine;

namespace UnderpinSlots
{
    // Create: Right Click > Create > UnderpinSlots > Slot Config
    // Save it as Assets/Resources/SlotConfig.asset (GameServices loads it by that name).
    [CreateAssetMenu(menuName = "UnderpinSlots/Slot Config", fileName = "SlotConfig")]
    public class SlotConfig : ScriptableObject, ISymbolCatalog
    {
        // Default values give about 89% RTP (return to player). Only the sprites must be assigned.
        [SerializeField]
        private List<SymbolDefinition> symbols = new List<SymbolDefinition>
        {
            new SymbolDefinition { symbol = SymbolType.Seven,  weight = 10, winMultiplier = 100 },
            new SymbolDefinition { symbol = SymbolType.Cherry, weight = 30, winMultiplier = 10  },
            new SymbolDefinition { symbol = SymbolType.Bell,   weight = 20, winMultiplier = 25  },
            new SymbolDefinition { symbol = SymbolType.Bar,    weight = 40, winMultiplier = 5   },
        };

        [SerializeField] private int[] betOptions = { 10, 50, 100, 500 };
        [SerializeField] private int startingBalance = 1000;

        public IReadOnlyList<SymbolDefinition> Symbols => symbols;
        public IReadOnlyList<int> BetOptions => betOptions;
        public int StartingBalance => startingBalance;

        public SymbolDefinition Get(SymbolType type)
        {
            foreach (var s in symbols)
                if (s.symbol == type) return s;
            throw new KeyNotFoundException($"Symbol {type} is not configured in SlotConfig.");
        }
    }
}
