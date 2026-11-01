using System;

namespace UnderpinSlots
{
    public interface ISettingsService
    {
        bool SoundOn { get; }
        event Action<bool> SoundChanged;
        void SetSound(bool on);
    }
}
