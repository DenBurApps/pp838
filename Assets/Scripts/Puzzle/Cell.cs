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

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }
        
        public void SetColorType(ColorTypes colorType)
        {
            _colorTypes = colorType;
            _internalImage.color = _cellDataHolder.GetColorByType(ColorType);
        }
        
        public void SwapColorWith(Cell otherCell)
        {
            var tempColor = _colorTypes;
            SetColorType(otherCell.ColorType);
            otherCell.SetColorType(tempColor);
        }

        private void OnButtonClicked() => Clicked?.Invoke(this);
    }
}
