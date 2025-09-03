using UnityEngine;

namespace ResourceCollector
{
    public class HarvesterEffects : HarvesterListener
    {
        [SerializeField] private ParticleSystem _collectParticles;
        [SerializeField] private AudioClip _collectSoundClip;
        
        protected override void RegisterEvents()
        {
            Harvester.OnCollectComplete += PlayCollectEffects;
        }

        protected override void UnregisterEvents()
        {
            Harvester.OnCollectComplete -= PlayCollectEffects;
        }

        private void PlayCollectEffects()
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