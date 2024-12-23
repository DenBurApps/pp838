using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mozaic
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Image _image;
            
        public ItemData ItemData { get; private set; }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
        public void SetItemData(ItemData data)
        {
            gameObject.SetActive(true);
            ItemData = data;
            _image.sprite = ItemData.Sprite;
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}