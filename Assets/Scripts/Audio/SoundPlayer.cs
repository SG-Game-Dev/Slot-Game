using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnderpinSlots
{
    // Plays sound effects and looping background music.
    // It lives in a prefab (Assets/Resources/SoundPlayer.prefab) that GameServices creates once
    // and keeps alive between scenes, so Home and Game share the same music and sounds.
    // Mute is NOT handled here: SettingsService sets AudioListener.volume, which silences everything.
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, ISoundPlayer
    {
        [Serializable]
        private struct Entry
        {
            public SoundId id;
            public AudioClip clip;
        }

        [Header("Sound effects")]
        [SerializeField] private List<Entry> sounds = new List<Entry>();
        [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

        [Header("Background music (optional)")]
        [SerializeField] private AudioClip music;
        [SerializeField, Range(0f, 1f)] private float musicVolume = 0.35f;

        private AudioSource sfxSource;
        private AudioSource musicSource;

        private void Awake()
        {
            sfxSource = GetComponent<AudioSource>();
            sfxSource.playOnAwake = false;

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = musicVolume;
            musicSource.clip = music;
            if (music != null) musicSource.Play();
        }

        public void Play(SoundId id)
        {
            foreach (var entry in sounds)
            {
                if (entry.id != id || entry.clip == null) continue;
                sfxSource.PlayOneShot(entry.clip, sfxVolume);
                return;
            }
            // No clip assigned for this id: silently ignore (the game works without sounds).
        }
    }
}