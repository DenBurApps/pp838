using System;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Image _internalImage;
        [SerializeField] private Button _button;
        [SerializeField] private CellDataHolder _cellDataHolder;
        [SerializeField] private ColorTypes _colorTypes;

        public event Action<Cell> Clicked;

        public ColorTypes ColorType => _colorTypes;

        public void SetColorType(ColorTypes colorType)
        {
            _colorTypes = colorType;
            _internalImage.color = _cellDataHolder.GetColorByType(ColorType);
        }

        private void OnButtonClicked() => Clicked?.Invoke(this);
    }
}
