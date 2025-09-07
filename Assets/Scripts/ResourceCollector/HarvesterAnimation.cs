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
            Harvester.MoveStarting += OnMoveStarting;
            Harvester.MoveStopped += OnMoveStopped;
            Harvester.CollectStarting += OnCollectStarting;
            Harvester.CollectCompleted += OnCollectCompleted;
        }

        protected override void UnregisterEvents()
        {
            Harvester.MoveStarting -= OnMoveStarting;
            Harvester.MoveStopped -= OnMoveStopped;
            Harvester.CollectStarting -= OnCollectStarting;
            Harvester.CollectCompleted -= OnCollectCompleted;
        }

        private void OnMoveStarting() => SetAnimatorBool(IsRunning, true);
        private void OnMoveStopped() => SetAnimatorBool(IsRunning, false);
        private void OnCollectStarting() => SetAnimatorBool(IsCollecting, true);
        private void OnCollectCompleted() => SetAnimatorBool(IsCollecting, false);

        private void SetAnimatorBool(string parameterName, bool value)
        {
            _animator.SetBool(parameterName, value);
        }
    }
}