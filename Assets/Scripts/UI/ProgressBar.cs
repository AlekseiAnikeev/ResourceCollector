using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ProgressBar : MonoBehaviour
    {
        [Range(0f, 1f)] [SerializeField] private float _hideDelay = 0.5f;
        [SerializeField] private Image _fillImage;

        private CanvasGroup _canvasGroup;
        private Coroutine _hideCoroutine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            Deactivate();
        }

        public void UpdateProgress(float progress)
        {
            _fillImage.fillAmount = Mathf.Clamp01(progress);

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);
        }

        public void Activate() => SetVisibility(true);

        public void Deactivate() => SetVisibility(false);

        private void SetVisibility(bool visible)
        {
            if (_canvasGroup == null)
                return;

            _canvasGroup.alpha = visible ? 1 : 0;

            if (visible == false)
                return;

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_hideDelay);

            Deactivate();
        }
    }
}