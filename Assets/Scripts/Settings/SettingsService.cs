using System;
using UnityEngine;

namespace UnderpinSlots
{
    public class SettingsService : ISettingsService
    {
        public bool SoundOn { get; private set; }
        public event Action<bool> SoundChanged = delegate { };

        public SettingsService(bool soundOn)
        {
            SoundOn = soundOn;
            Apply();
        }

        public void SetSound(bool on)
        {
            if (on == SoundOn) return;
            SoundOn = on;
            Apply();
            SoundChanged(on);
        }

        // Global mute: works for any AudioSource added later.
        private void Apply() => AudioListener.volume = SoundOn ? 1f : 0f;
    }
}
