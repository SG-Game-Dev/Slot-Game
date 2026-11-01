using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // Bet label + (-) and (+) buttons. Left arrow = decrease, right arrow = increase.
    public class BetView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Button decreaseButton;
        [SerializeField] private Button increaseButton;

        private IBetSelector selector;
        private bool interactable = true;

        private void Awake()
        {
            decreaseButton.onClick.AddListener(() => selector?.Decrease());
            increaseButton.onClick.AddListener(() => selector?.Increase());
        }

        public void Bind(IBetSelector betSelector)
        {
            Unbind();
            selector = betSelector;
            selector.BetChanged += OnBetChanged;
            Refresh();
        }

        // Disabled while the reels are spinning.
        public void SetInteractable(bool value)
        {
            interactable = value;
            Refresh();
        }

        private void OnBetChanged(int _) => Refresh();

        private void Refresh()
        {
            if (selector == null) return;
            label.text = "BET: " + selector.CurrentBet;
            decreaseButton.interactable = interactable && selector.CanDecrease;
            increaseButton.interactable = interactable && selector.CanIncrease;
        }

        private void Unbind()
        {
            if (selector != null) selector.BetChanged -= OnBetChanged;
        }

        private void OnDestroy() => Unbind();
    }
}
