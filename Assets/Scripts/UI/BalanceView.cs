using System.Collections;
using TMPro;
using UnityEngine;

namespace UnderpinSlots
{
    // Shows the wallet balance and counts smoothly up/down when it changes.
    public class BalanceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private string prefix = "BALANCE: ";
        [SerializeField] private float countDuration = 0.6f;

        private IWallet wallet;
        private float displayed;
        private int target;
        private Coroutine countRoutine;

        public void Bind(IWallet walletToShow)
        {
            Unbind();
            wallet = walletToShow;
            wallet.BalanceChanged += OnBalanceChanged;

            target = wallet.Balance;
            displayed = target;
            Render();
        }

        private void OnBalanceChanged(int balance)
        {
            target = balance;

            if (!isActiveAndEnabled) { displayed = balance; Render(); return; }

            if (countRoutine != null) StopCoroutine(countRoutine);
            countRoutine = StartCoroutine(CountTo());
        }

        private IEnumerator CountTo()
        {
            float start = displayed;
            // unscaledDeltaTime: keeps working even while the game is paused.
            for (float t = 0f; t < countDuration; t += Time.unscaledDeltaTime)
            {
                displayed = Mathf.Lerp(start, target, t / countDuration);
                Render();
                yield return null;
            }
            displayed = target;
            Render();
        }

        private void Render() => label.text = prefix + Mathf.RoundToInt(displayed);

        private void Unbind()
        {
            if (wallet != null) wallet.BalanceChanged -= OnBalanceChanged;
        }

        private void OnDestroy() => Unbind();
    }
}
