using System;
using UnityEngine;

namespace UnderpinSlots
{
    // Designer-editable data for one symbol.
    [Serializable]
    public struct SymbolDefinition
    {
        public SymbolType symbol;
        public Sprite sprite;

        [Tooltip("Higher = appears more often on a reel.")]
        [Min(0)] public int weight;

        [Tooltip("Prize = bet x this value when all reels show the symbol.")]
        [Min(0)] public int winMultiplier;
    }
}
