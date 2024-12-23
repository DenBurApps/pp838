using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class CellDataHolder : MonoBehaviour
    {
        [SerializeField] private List<CellColorHolder> _cellColorHolder;

        public Color GetColorByType(ColorTypes type)
        {
            foreach (var color in _cellColorHolder)
            {
                if (color.ColorType == type)
                    return color.Color;
            }
            
            return Color.white;
        }
    }

    [Serializable]
    public class CellColorHolder
    {
        public ColorTypes ColorType;
        public Color Color;
    }

    public enum ColorTypes
    {
        Blue,
        Pink,
        Grey,
        Coral,
        Orange,
        Yellow,
        White
    }
}