using UnityEngine;
using UnityEngine.AI;

namespace ResourceCollector
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class HarvesterPathVisualizer : HarvesterListener
    {
        private LineRenderer _lineRenderer;
        private NavMeshAgent _agent;
        
        private bool _isActive;

        protected override void Awake()
        {
            base.Awake();
            
            _lineRenderer = GetComponent<LineRenderer>();
            _agent = GetComponent<NavMeshAgent>();
            
            _lineRenderer.positionCount = 0;
        }

        private void Update()
        {
            if (_isActive && _agent.hasPath)
            {
                _lineRenderer.positionCount = _agent.path.corners.Length;
                _lineRenderer.SetPositions(_agent.path.corners);
            }
        }

        protected override void RegisterEvents()
        {
            Harvester.MoveStarting += OnMoveStarting;
            Harvester.MoveStopped += OnMoveStopped;
        }

        protected override void UnregisterEvents()
        {
            Harvester.MoveStarting -= OnMoveStarting;
            Harvester.MoveStopped -= OnMoveStopped;
        }

        private void OnMoveStarting()
        {
            _isActive = true;
        }

        private void OnMoveStopped()
        {
            _isActive = false;
            _lineRenderer.positionCount = 0;
        }
    }
}