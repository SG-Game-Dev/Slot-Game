using System;

namespace UnderpinSlots
{
    // Everything that is saved locally.
    [Serializable]
    public class PlayerData
    {
        public int balance;
        public int bet;
        public bool soundOn = true;

        public static PlayerData CreateDefault(int startingBalance, int defaultBet)
        {
            return new PlayerData { balance = startingBalance, bet = defaultBet, soundOn = true };
        }
    }
}
