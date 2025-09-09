using UI;
using UnityEngine;

namespace ResourceCollector
{
    public class HarvesterProgressUI : HarvesterListener
    {
        [SerializeField] private ProgressBar _progressBar;

        protected override void OnEnable()
        {
            base.OnEnable();
            _progressBar?.Deactivate();
        }

        protected override void RegisterEvents()
        {
            Harvester.CollectStarting += OnCollectStarting;
            Harvester.CollectingProgress += OnCollectingProgress;
            Harvester.CollectCompleted += OnCollectCompleted;
            Harvester.IdleStarted += OnIdleStarted;
        }

        protected override void UnregisterEvents()
        {
            Harvester.CollectStarting -= OnCollectStarting;
            Harvester.CollectingProgress -= OnCollectingProgress;
            Harvester.CollectCompleted -= OnCollectCompleted;
            Harvester.IdleStarted -= OnIdleStarted;
        }

        private void OnCollectStarting()
        {
            _progressBar?.Activate();
            _progressBar?.UpdateProgress(0f);
        }

        private void OnCollectingProgress(float progress)
        {
            _progressBar?.UpdateProgress(progress);
        }

        private void OnCollectCompleted()
        {
            _progressBar?.UpdateProgress(1f);
            _progressBar?.Deactivate();
        }

        private void OnIdleStarted()
        {
            _progressBar?.Deactivate();
        }
    }
}