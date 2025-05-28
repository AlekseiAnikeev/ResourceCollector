using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] private float hideDelay = 0.5f;

        [Header("Зависимости")]
        [SerializeField] private Image fillImage;
        [SerializeField] private CanvasGroup canvasGroup;

        private Coroutine _hideCoroutine;

        private void Awake()
        {
            if(canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            
            SetVisibility(false);
        }

        public void UpdateProgress(float progress)
        {
            fillImage.fillAmount = Mathf.Clamp01(progress);
            if(_hideCoroutine != null) 
                StopCoroutine(_hideCoroutine);
        }

        public void SetVisibility(bool visible)
        {
            if(canvasGroup == null) return;
        
            canvasGroup.alpha = visible ? 1 : 0;
            if(visible)
                _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(hideDelay);
            canvasGroup.alpha = 0;
        }
    }
}