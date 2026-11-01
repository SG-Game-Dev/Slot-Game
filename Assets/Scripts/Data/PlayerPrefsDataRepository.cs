using UnityEngine;

namespace UnderpinSlots
{
    // Stores PlayerData as JSON in PlayerPrefs (works on desktop AND WebGL, where it uses the browser storage).
    public class PlayerPrefsDataRepository : IPlayerDataRepository
    {
        private const string Key = "underpin.slots.player_data";

        public bool TryLoad(out PlayerData data)
        {
            data = null;
            if (!PlayerPrefs.HasKey(Key)) return false;

            try
            {
                data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(Key));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Saved data is corrupted, starting fresh. " + e.Message);
            }
            return data != null;
        }

        public void Save(PlayerData data)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(Key);
            PlayerPrefs.Save();
        }
    }
}
