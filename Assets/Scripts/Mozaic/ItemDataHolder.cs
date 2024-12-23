using System;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mozaic
{
    public class ItemDataHolder : MonoBehaviour
    {
        [SerializeField] private List<ItemData> _itemDatas;
        [SerializeField] private List<ColorHolder> _colors;

        public ItemData GetRandomItem()
        {
            var randomIndex = Random.Range(0, _itemDatas.Count);
            
            var itemDataToReturn = _itemDatas[randomIndex];
            itemDataToReturn.ColorType = _colors[randomIndex].ColorType;
            return _itemDatas[randomIndex];
        }

        public Color GetColorByType(ColorType type)
        {
            foreach (var color in _colors)
            {
                if (color.ColorType == type)
                {
                    return color.Color;
                }
            }

            return Color.white;
        }

        public Sprite GetSpriteByType(FigureType type)
        {
            foreach (var itemData in _itemDatas)
            {
                if (itemData.FigureType == type)
                {
                    return itemData.Sprite;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class ColorHolder
    {
        public Color Color;
        public ColorType ColorType;
    }

    [Serializable]
    public class ItemData
    {
        public Sprite Sprite;
        public FigureType FigureType;
        public ColorType ColorType;
    }

    public enum FigureType
    {
        Square,
        Pentagon,
        Star,
        Heart,
        Rectangle,
        Triangle,
        Oval,
        Moon,
        Circle,
        None
    }
    
    public enum ColorType
    {
        Pink,
        Cyan,
        Grey,
        Yellow,
        Green,
        Blue,
        Beig,
        Black,
        Orange,
        None
    }
}
