using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // The lever. Visual = two sprites (up / pulled). Click area = a small transparent Button
    // over the red ball, so clicks on the rest of the machine do nothing.
    public class SlotHandle : MonoBehaviour
    {
        [SerializeField] private Button hitArea;
        [SerializeField] private Image leverImage;
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite pulledSprite;
        [SerializeField] private float pullTime = 0.25f;

        public event Action Clicked = delegate { };

        private void Awake()
        {
            leverImage.sprite = idleSprite;
            hitArea.onClick.AddListener(() => Clicked());
        }

        public void SetInteractable(bool value) => hitArea.interactable = value;

        // Shows the pulled sprite briefly, then returns to idle.
        public IEnumerator PlayPull()
        {
            leverImage.sprite = pulledSprite;
            yield return new WaitForSeconds(pullTime);
            leverImage.sprite = idleSprite;
        }
    }
}
