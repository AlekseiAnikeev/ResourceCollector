using UnityEngine;

namespace ResourceCollector
{
    [RequireComponent(typeof(Harvester))]
    public abstract class HarvesterListener : MonoBehaviour
    {
        protected Harvester Harvester { get; private set; }

        protected virtual void Awake()
        {
            Harvester = GetComponent<Harvester>();
        }

        protected virtual void OnEnable()
        {
            RegisterEvents();
        }

        protected virtual void OnDisable()
        {
            UnregisterEvents();
        }

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();
    }
}