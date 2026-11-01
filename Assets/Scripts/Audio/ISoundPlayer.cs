namespace UnderpinSlots
{
    public interface ISoundPlayer
    {
        void Play(SoundId id);
    }

    // Every sound the game can play.
    public enum SoundId
    {
        Spin,      // lever pulled / reels start
        ReelStop,  // each reel stops
        Win,       // winning result
        Click      // bet changed / button pressed
    }
}