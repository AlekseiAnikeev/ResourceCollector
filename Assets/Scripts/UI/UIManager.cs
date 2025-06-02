using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourcesText;
        [SerializeField] private SupplyCenter _mainBase;

        private void OnEnable()
        {
            _mainBase.OnResourcesChanged += UpdateResourcesUI;
        }

        private void OnDisable()
        {
            _mainBase.OnResourcesChanged -= UpdateResourcesUI;
        }

        private void UpdateResourcesUI(int amount)
        {
            _resourcesText.text = $"Кристалов: {amount}";
        }
    }
}