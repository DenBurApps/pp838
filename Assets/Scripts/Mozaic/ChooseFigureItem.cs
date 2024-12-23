using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mozaic
{
    public class ChooseFigureItem : MonoBehaviour
    {
        [SerializeField] private Sprite _defaultSprite;
        
        [SerializeField] private Image _image;
        [SerializeField] private Button _openButton;
        [SerializeField] private TMP_Text _correctText;
        [SerializeField] private TMP_Text _uncorrectText;
        [SerializeField] private GameObject _sparkleImage;
        [SerializeField] private ItemDataHolder _itemDataHolder;
        
        public event Action<ChooseFigureItem> ButtonClicked;
        
        public FigureType FigureType { get; private set; }
        public ColorType ColorType { get; private set; }

        private void OnEnable()
        {
            _openButton.onClick.AddListener(OnButtonClicked);

            _image.sprite = _defaultSprite;
        }

        private void OnDisable()
        {
            _openButton.onClick.RemoveListener(OnButtonClicked);
            ResetAllElements();
        }

        public void SetFigure(FigureType figureType)
        {
            FigureType = figureType;
            _image.sprite = _itemDataHolder.GetSpriteByType(FigureType);
        }

        public void SetColor(ColorType figureColor)
        {
            ColorType = figureColor;
            _image.color = _itemDataHolder.GetColorByType(ColorType);
        }

        public void SetCorrectData(ItemData itemData)
        {
            if(_image.sprite != _defaultSprite)
                return;
            
            _image.sprite = itemData.Sprite;
            _image.color = _itemDataHolder.GetColorByType(itemData.ColorType);
            
            _sparkleImage.SetActive(true);
        }

        public bool ImageSelected()
        {
            return _image.sprite != _defaultSprite;
        }

        public bool VerifyResult(ItemData itemData)
        {
            if (FigureType == itemData.FigureType && ColorType == itemData.ColorType)
            {
                _correctText.enabled = true;
                return true;
            }

            _uncorrectText.enabled = true;
            return false;
        }
        
        private void OnButtonClicked()
        {
            ButtonClicked?.Invoke(this);
        }

        private void ResetAllElements()
        {
            FigureType = FigureType.None;
            ColorType = ColorType.None;
            _image.sprite = _defaultSprite;
            _image.color = Color.white;
            _correctText.enabled = false;
            _uncorrectText.enabled = false;
            _sparkleImage.SetActive(false);
        }
    }
}
