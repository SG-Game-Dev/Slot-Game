using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // Add this to any Button to play the Click sound when it is pressed.
    // (Do NOT add it to the Bet -/+ arrows: the game already plays Click when the bet changes.)
    [RequireComponent(typeof(Button))]
    public class UiClickSound : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => GameServices.Sound.Play(SoundId.Click));
        }
    }
}