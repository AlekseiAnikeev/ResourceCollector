using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Deactivate();
        }

        public void UpdateProgress(float progress)
        {
            _fillImage.fillAmount = Mathf.Clamp01(progress);
        }

        public void Activate() => SetVisibility(true);
        public void Deactivate() => SetVisibility(false);

        private void SetVisibility(bool visible)
        {
            if (_canvasGroup == null)
                return;

            _canvasGroup.alpha = visible ? 1 : 0;
        }
    }
}