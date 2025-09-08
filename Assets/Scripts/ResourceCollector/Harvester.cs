using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ResourceCollector
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Harvester : MonoBehaviour, ITrackable<Harvester>
    {
        [SerializeField] private float _collectionDistance = 0.5f;
        [SerializeField] private float _collectionDelay = 2f;
        [SerializeField] private float _heightResourcePinning = 2f;

        private NavMeshAgent _agent;
        private Resource _targetResource;
        private Transform _carryPoint;
        private SupplyCenter _homeSupplyCenter;
        private Coroutine _harvestCoroutine;

        public event Action MoveStarting;
        public event Action MoveStopped;
        public event Action CollectStarting;
        public event Action<float> CollectingProgress;
        public event Action CollectCompleted;
        public event Action IdleStarted;
        public event Action<Resource> ResourceDelivered;
        public event Action<Harvester> Collected;

        public bool IsAvailable { get; private set; } = true;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _carryPoint = transform;
        }

        public void Init(SupplyCenter center)
        {
            _homeSupplyCenter = center;
        }

        public void Collect(Resource resource)
        {
            if (IsAvailable == false || resource == null)
                return;

            IsAvailable = false;
            _targetResource = resource;

            _agent.SetDestination(_targetResource.transform.position);
            MoveStarting?.Invoke();

            if (_harvestCoroutine != null)
            {
                StopCoroutine(_harvestCoroutine);
            }

            StartCoroutine(Harvest());
        }

        private void ResetState()
        {
            if (_targetResource != null)
            {
                _targetResource.transform.SetParent(null);
                _targetResource = null;
            }

            IsAvailable = true;
            IdleStarted?.Invoke();
        }

        private IEnumerator Harvest()
        {
            yield return MoveToResource();
            yield return CollectResource();
            yield return ReturnToBase();
            ResetState();
        }

        private IEnumerator MoveToResource()
        {
            yield return new WaitUntil(() =>
                Vector3.SqrMagnitude(transform.position - _targetResource.transform.position) <=
                _collectionDistance * _collectionDistance);

            _agent.isStopped = true;
            MoveStopped?.Invoke();
            CollectStarting?.Invoke();
        }

        private IEnumerator CollectResource()
        {
            float timer = 0f;
            while (timer < _collectionDelay)
            {
                if (_targetResource == null)
                    yield break;

                CollectingProgress?.Invoke(timer / _collectionDelay);
                timer += Time.deltaTime;
                yield return null;
            }

            CollectingProgress?.Invoke(1f);
            _agent.isStopped = false;

            if (_targetResource == null)
            {
                ResetState();
                yield break;
            }

            _targetResource.Collect();
            _targetResource.transform.SetParent(_carryPoint);
            _targetResource.transform.localPosition = Vector3.up * _heightResourcePinning;

            CollectCompleted?.Invoke();
        }

        private IEnumerator ReturnToBase()
        {
            _agent.SetDestination(_homeSupplyCenter.transform.position);
            MoveStarting?.Invoke();

            yield return new WaitUntil(() =>
                Vector3.SqrMagnitude(transform.position - _homeSupplyCenter.transform.position) <=
                _collectionDistance * _collectionDistance);

            MoveStopped?.Invoke();
            ResourceDelivered?.Invoke(_targetResource);
        }
    }
}