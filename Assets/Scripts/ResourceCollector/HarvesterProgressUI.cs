using UI;
using UnityEngine;

namespace ResourceCollector
{
    public class HarvesterProgressUI : HarvesterListener
    {
        [SerializeField] private ProgressBar _progressBar;

        protected override void Awake()
        {
            base.Awake();
            _progressBar?.Deactivate();
        }

        protected override void RegisterEvents()
        {
            Harvester.OnCollectStart += ShowProgressStart;
            Harvester.OnCollectProgress += UpdateProgress;
            Harvester.OnCollectComplete += ShowProgressComplete;
            Harvester.OnIdle += HideProgress;
        }

        protected override void UnregisterEvents()
        {
            Harvester.OnCollectStart -= ShowProgressStart;
            Harvester.OnCollectProgress -= UpdateProgress;
            Harvester.OnCollectComplete -= ShowProgressComplete;
            Harvester.OnIdle -= HideProgress;
        }

        private void ShowProgressStart()
        {
            _progressBar?.Activate();
            _progressBar?.UpdateProgress(0f);
        }

        private void UpdateProgress(float progress)
        {
            _progressBar?.UpdateProgress(progress);
        }

        private void ShowProgressComplete()
        {
            _progressBar?.UpdateProgress(1f);
            _progressBar?.Deactivate();
        }

        private void HideProgress()
        {
            _progressBar?.Deactivate();
        }
    }
}