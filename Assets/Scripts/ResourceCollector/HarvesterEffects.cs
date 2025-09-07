using UnityEngine;

namespace ResourceCollector
{
    public class HarvesterEffects : HarvesterListener
    {
        [SerializeField] private ParticleSystem _collectParticles;
        [SerializeField] private AudioClip _collectSoundClip;

        protected override void RegisterEvents() =>
            Harvester.CollectCompleted += OnCollectCompleted;

        protected override void UnregisterEvents() =>
            Harvester.CollectCompleted -= OnCollectCompleted;

        private void OnCollectCompleted()
        {
            if (_collectParticles != null)
            {
                _collectParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _collectParticles.Play();
            }

            if (_collectSoundClip != null)
                AudioSource.PlayClipAtPoint(_collectSoundClip, transform.position);
        }
    }
}