using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UnderpinSlots
{
    // UI reel: a column of symbol Images scrolling inside a RectMask2D window.
    //
    // The whole reel position is ONE number, "offset", measured in slots:
    //   offset = 2.0  -> strip[2] sits exactly on the pay line (window centre)
    //   offset = 2.5  -> halfway between strip[2] and strip[3]
    // A small pool of Images is re-used and re-assigned every frame, so the reel can loop forever.
    // All animation is time based (deltaTime), so it looks the same at any frame rate.
    public class Reel : MonoBehaviour, IReel
    {
        [Header("Layout (pixels)")]
        [SerializeField] private float slotHeight = 100f;
        [SerializeField] private Vector2 symbolSize = new Vector2(100f, 100f);

        [Header("Animation")]
        [SerializeField] private float maxSpeed = 14f;      // slots per second
        [SerializeField] private float spinUpTime = 0.4f;   // seconds to reach max speed
        [SerializeField] private float stopDuration = 1.2f; // seconds of slow-down

        private const int PoolRadius = 2;                   // pool = 5 images: -2..+2 around the pay line
        private const float Overshoot = 1f;                 // small bounce when stopping
        // Slope of the ease-out-back curve at t = 0; used to start slowing down at exactly maxSpeed (no jerk).
        private const float EaseStartSlope = 3f * (Overshoot + 1f) - 2f * Overshoot;

        private SymbolType[] strip;
        private Sprite[] stripSprites;
        private Image[] pool;
        private float offset;

        public bool IsStopped { get; private set; } = true;
        public SymbolType StoppedSymbol { get; private set; }

        public void Initialize(ISymbolCatalog catalog)
        {
            if (GetComponent<RectMask2D>() == null) gameObject.AddComponent<RectMask2D>();

            strip = catalog.Symbols.Select(s => s.symbol).ToArray();
            stripSprites = catalog.Symbols.Select(s => s.sprite).ToArray();

            BuildPool();

            offset = Random.Range(0, strip.Length); // start every reel on a random symbol
            StoppedSymbol = strip[Mod(Mathf.RoundToInt(offset), strip.Length)];
            Render();
        }

        public void Spin(SymbolType target, float runTime)
        {
            if (!IsStopped) return;
            StartCoroutine(SpinRoutine(target, runTime));
        }

        private IEnumerator SpinRoutine(SymbolType target, float runTime)
        {
            IsStopped = false;

            // 1) Spin up: ease the speed from 0 to maxSpeed.
            for (float t = 0f; t < spinUpTime; t += Time.deltaTime)
            {
                float speed = maxSpeed * Mathf.SmoothStep(0f, 1f, t / spinUpTime);
                offset += speed * Time.deltaTime;
                Render();
                yield return null;
            }

            // 2) Cruise at full speed. Each reel gets a different runTime, so they stop one after another.
            for (float t = 0f; t < runTime; t += Time.deltaTime)
            {
                offset += maxSpeed * Time.deltaTime;
                Render();
                yield return null;
            }

            // 3) Slow down and land exactly on the pre-decided target symbol.
            float start = offset;
            float minDistance = maxSpeed * stopDuration / EaseStartSlope;
            float end = NextOffsetFor(target, start + minDistance);

            for (float t = 0f; t < stopDuration; t += Time.deltaTime)
            {
                offset = Mathf.LerpUnclamped(start, end, EaseOutBack(t / stopDuration));
                Render();
                yield return null;
            }

            offset = Mod(Mathf.RoundToInt(end), strip.Length); // keep numbers small
            Render();

            StoppedSymbol = target;
            IsStopped = true;
        }

        // Smallest whole offset >= minOffset that puts the target symbol on the pay line.
        private float NextOffsetFor(SymbolType target, float minOffset)
        {
            int n = strip.Length;
            int index = System.Array.IndexOf(strip, target);
            float candidate = Mathf.Floor(minOffset / n) * n + index;
            if (candidate < minOffset) candidate += n;
            return candidate;
        }

        // Fast start, slow end, small overshoot ("settling" feel). t is 0..1.
        private static float EaseOutBack(float t)
        {
            t = Mathf.Clamp01(t) - 1f;
            return 1f + (Overshoot + 1f) * t * t * t + Overshoot * t * t;
        }

        private void BuildPool()
        {
            pool = new Image[PoolRadius * 2 + 1];
            for (int i = 0; i < pool.Length; i++)
            {
                var go = new GameObject("Symbol_" + i, typeof(RectTransform), typeof(Image));
                go.transform.SetParent(transform, false);

                var rect = (RectTransform)go.transform;
                rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = symbolSize;

                var image = go.GetComponent<Image>();
                image.raycastTarget = false;
                image.preserveAspect = true;
                pool[i] = image;
            }
        }

        // Places every pooled image for the current offset.
        private void Render()
        {
            int n = strip.Length;
            float whole = Mathf.Floor(offset);
            float fraction = offset - whole;

            for (int i = 0; i < pool.Length; i++)
            {
                int r = i - PoolRadius;                                   // -2..+2
                pool[i].sprite = stripSprites[Mod((int)whole + r, n)];
                // Increasing offset moves symbols DOWN (y decreases).
                pool[i].rectTransform.anchoredPosition = new Vector2(0f, -(fraction - r) * slotHeight);
            }
        }

        private static int Mod(int value, int length) => ((value % length) + length) % length;
    }
}
