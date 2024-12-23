using System;
using System.Collections.Generic;
using UnityEngine;

namespace Memory
{
    public class CellHolder : MonoBehaviour
    {
        [SerializeField] private List<Cell> _cells;

        public event Action<Cell> CellClicked;
        
        public List<Cell> Cells => _cells;

        private void OnEnable()
        {
            foreach (var cell in _cells)
            {
                cell.Clicked += OnCellClicked;
            }
        }

        private void OnDisable()
        {
            foreach (var cell in _cells)
            {
                cell.Clicked -= OnCellClicked;
            }
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void SetSpriteProvider(CellSpriteProvider spriteProvider)
        {
            foreach (var cell in _cells)
            {
                cell.SetCellSpriteProvider(spriteProvider);
            }
        }

        public void ShowAllCells()
        {
            foreach (var cell in _cells)
            {
                cell.ShowCellImage();
            }
        }

        public void HideAllCellsImages()
        {
            foreach (var cell in _cells)
            {
                cell.HideCellImage();
            }
        }

        private void OnCellClicked(Cell cell) => CellClicked?.Invoke(cell);
    }
}
