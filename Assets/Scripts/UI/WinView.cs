using TMPro;
using UnityEngine;

namespace UnderpinSlots
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        public void Clear() => label.text = "WIN: 0";
        public void Show(int amount) => label.text = "WIN: " + amount;
        public void ShowMessage(string message) => label.text = message;
    }
}
