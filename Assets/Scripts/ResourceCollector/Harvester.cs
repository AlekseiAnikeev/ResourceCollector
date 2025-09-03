using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ResourceCollector
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Harvester : MonoBehaviour, ITrackable<Harvester>
    {
        public event Action OnMoveStart;
        public event Action OnMoveStop;
        public event Action OnCollectStart;
        public event Action<float> OnCollectProgress;
        public event Action OnCollectComplete;
        public event Action OnIdle;
        public event Action<Resource> OnResourceDelivered;
        public event Action<Harvester> Collected;

        [SerializeField] private float _collectionDistance = 0.5f;
        [SerializeField] private float _collectionDelay = 2f;
        [SerializeField] private float _heightResourcePinning = 2f;

        private NavMeshAgent _agent;
        private Resource _targetResource;
        private Transform _carryPoint;
        private SupplyCenter _homeSupplyCenter;
        private Coroutine _harvestCoroutine;

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
            _targetResource.MarkAsTargeted();

            _agent.SetDestination(_targetResource.transform.position);
            OnMoveStart?.Invoke();

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
            OnIdle?.Invoke();
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
            OnMoveStop?.Invoke();
            OnCollectStart?.Invoke();
        }

        private IEnumerator CollectResource()
        {
            float timer = 0f;
            while (timer < _collectionDelay)
            {
                if (_targetResource == null || _targetResource.IsCollected)
                    yield break;

                OnCollectProgress?.Invoke(timer / _collectionDelay);
                timer += Time.deltaTime;
                yield return null;
            }

            OnCollectProgress?.Invoke(1f);
            _agent.isStopped = false;

            if (_targetResource == null || _targetResource.IsCollected)
            {
                ResetState();
                yield break;
            }

            _targetResource.MarkAsCollected();
            _targetResource.transform.SetParent(_carryPoint);
            _targetResource.transform.localPosition = Vector3.up * _heightResourcePinning;

            OnCollectComplete?.Invoke();
        }

        private IEnumerator ReturnToBase()
        {
            _agent.SetDestination(_homeSupplyCenter.transform.position);
            OnMoveStart?.Invoke();

            yield return new WaitUntil(() =>
                Vector3.SqrMagnitude(transform.position - _homeSupplyCenter.transform.position) <=
                _collectionDistance * _collectionDistance);

            OnMoveStop?.Invoke();
            OnResourceDelivered?.Invoke(_targetResource);
        }
    }
}