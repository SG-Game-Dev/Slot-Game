namespace UnderpinSlots
{
    // Does nothing. Used when no SoundPlayer prefab exists, so callers never need null checks.
    public class NullSoundPlayer : ISoundPlayer
    {
        public void Play(SoundId id) { }
    }
}