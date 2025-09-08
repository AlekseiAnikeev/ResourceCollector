using System;
using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanInterval = 3f;
    [SerializeField] private float _scanRadius = 10f;
    [SerializeField] private LayerMask _scanLayer;

    private WaitForSeconds _scanDelay;
    private Coroutine _scanCoroutine;

    public event Action<Collider[]> ScanCompleted;

    private void Awake()
    {
        _scanDelay = new WaitForSeconds(_scanInterval);
    }

    private void OnEnable()
    {
        if (_scanCoroutine != null)
            StopCoroutine(_scanCoroutine);

        _scanCoroutine = StartCoroutine(ScanCoroutine());
    }

    private void OnDestroy()
    {
        if (_scanCoroutine != null)
            StopCoroutine(_scanCoroutine);
    }

    private IEnumerator ScanCoroutine()
    {
        while (enabled)
        {
            yield return _scanDelay;
            Scan();
        }
    }

    private void Scan()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius, _scanLayer);
        ScanCompleted?.Invoke(hits);
    }
}