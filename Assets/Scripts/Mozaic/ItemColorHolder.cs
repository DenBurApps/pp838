using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mozaic
{
    public class ItemColorHolder : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private ColorType _colorType;
        [SerializeField] private ItemDataHolder _itemDataHolder;

        public event Action<ColorType> ColorSelected; 
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
            _image.color = _itemDataHolder.GetColorByType(_colorType);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }
        
        public void SetItemData(FigureType image)
        {
            _image.sprite = _itemDataHolder.GetSpriteByType(image);
        }

        private void OnButtonClicked() => ColorSelected?.Invoke(_colorType);
    }
}
