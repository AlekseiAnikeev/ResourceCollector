using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.AI;

public class Harvester : MonoBehaviour
{
    private const string CommandMove = "Move";
    private const string CommandCollect = "Collect";
    private const string CommandIdle = "Idle";

    [Header("Настройки")]
    [SerializeField] private float _collectionDistance = 0.5f;
    [SerializeField] private float _collectionDelay = 2f;

    [Header("Зависимости")]
    [SerializeField] private ParticleSystem _collectingParticles;
    [SerializeField] private LineRenderer _pathRenderer;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _collectSound;

    public event Action<Resource> OnResourceDelivered;
    public bool IsAvailable { get; private set; } = true;

    private SupplyCenter _homeSupplyCenter;
    private NavMeshAgent _agent;
    private Resource _currentTarget;

    private Coroutine _collectCoroutine;
    private Coroutine _pathCoroutine;
    private Coroutine _deliverCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _pathRenderer.positionCount = 0;
        _progressBar?.SetVisibility(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Init(SupplyCenter center)
    {
        _homeSupplyCenter = center;
    }

    public void AssignResource(Resource resource)
    {
        if (!IsAvailable) 
            return;

        IsAvailable = false;
        _currentTarget = resource;
        _currentTarget.MarkAsTargeted();

        _agent.SetDestination(_currentTarget.transform.position);
        _animator.SetBool(CommandMove, true);

        RestartCoroutine(ref _collectCoroutine, CollectResource());
        RestartCoroutine(ref _pathCoroutine, UpdatePathVisualization());
    }

    private IEnumerator CollectResource()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, _currentTarget.transform.position) <= _collectionDistance);

        _agent.isStopped = true;
        _animator.SetBool(CommandCollect, true);

        float timer = 0f;
        _progressBar?.SetVisibility(true);

        while (timer < _collectionDelay)
        {
            if (_currentTarget == null || _currentTarget.IsCollected)
                break;

            _progressBar?.UpdateProgress(timer / _collectionDelay);
            timer += Time.deltaTime;
            yield return null;
        }

        _progressBar?.SetVisibility(false);
        _agent.isStopped = false;

        if (_currentTarget != null && !_currentTarget.IsCollected)
        {
            PlayCollectEffects();
            ReturnToBase();
        }
        else
        {
            ResetState();
        }
    }

    private IEnumerator DeliverResource()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, _homeSupplyCenter.transform.position) <= _collectionDistance);

        OnResourceDelivered?.Invoke(_currentTarget);
        ResetState();
    }

    private IEnumerator UpdatePathVisualization()
    {
        while (IsAvailable == false)
        {
            if (_agent.hasPath)
            {
                _pathRenderer.positionCount = _agent.path.corners.Length;
                _pathRenderer.SetPositions(_agent.path.corners);
            }
            yield return new WaitForSeconds(0.1f);
        }
        _pathRenderer.positionCount = 0;
    }

    private void ReturnToBase()
    {
        _currentTarget.transform.SetParent(transform);
        _currentTarget.transform.localPosition = Vector3.up * 2f;

        _animator.SetBool(CommandCollect, false);
        _agent.SetDestination(_homeSupplyCenter.transform.position);

        RestartCoroutine(ref _deliverCoroutine, DeliverResource());
    }

    private void ResetState()
    {
        if (_currentTarget != null)
        {
            _currentTarget.transform.SetParent(null);
            _currentTarget = null;
        }

        IsAvailable = true;
        _animator.SetBool(CommandMove, false);
        _animator.SetTrigger(CommandIdle);
        _pathRenderer.positionCount = 0;
    }

    private void PlayCollectEffects()
    {
        if (_collectingParticles != null)
        {
            _collectingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _collectingParticles.Play();
        }
        AudioSource.PlayClipAtPoint(_collectSound, transform.position);
    }

    private void RestartCoroutine(ref Coroutine coroutine, IEnumerator method)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(method);
    }
}
