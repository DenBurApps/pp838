using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mozaic
{
    public class ItemTypeHolder : MonoBehaviour
    {
        [SerializeField] private FigureType _figureType;
        [SerializeField] private Button _button;

        public event Action<FigureType> TypeSelected;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked() => TypeSelected?.Invoke(_figureType);
    }
}
