using UnityEngine;

namespace ResourceCollector
{
    [RequireComponent(typeof(Animator))]
    public class HarvesterAnimation : HarvesterListener
    {
        private const string IsRunning = "IsRunning";
        private const string IsCollecting = "IsCollecting";

        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();
        }

        protected override void RegisterEvents()
        {
            Harvester.OnMoveStart += PlayMoveAnimation;
            Harvester.OnMoveStop += StopMoveAnimation;
            Harvester.OnCollectStart += PlayCollectAnimation;
            Harvester.OnCollectComplete += StopCollectAnimation;
        }

        protected override void UnregisterEvents()
        {
            Harvester.OnMoveStart -= PlayMoveAnimation;
            Harvester.OnMoveStop -= StopMoveAnimation;
            Harvester.OnCollectStart -= PlayCollectAnimation;
            Harvester.OnCollectComplete -= StopCollectAnimation;
        }

        private void PlayMoveAnimation() => SetAnimatorBool(IsRunning, true);
        private void StopMoveAnimation() => SetAnimatorBool(IsRunning, false);
        private void PlayCollectAnimation() => SetAnimatorBool(IsCollecting, true);
        private void StopCollectAnimation() => SetAnimatorBool(IsCollecting, false);

        private void SetAnimatorBool(string parameterName, bool value)
        {
            _animator.SetBool(parameterName, value);
        }
    }
}