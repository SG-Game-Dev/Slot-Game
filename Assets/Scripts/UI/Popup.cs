using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // Base class for every popup. Keep popups as direct children of the Canvas so
    // SetAsLastSibling() always draws the newest popup on top.
    public class Popup : MonoBehaviour
    {
        [SerializeField] private Button closeButton; // optional (the X button)

        protected virtual void Awake()
        {
            if (closeButton != null) closeButton.onClick.AddListener(Close);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public virtual void Close() => gameObject.SetActive(false);
    }
}
