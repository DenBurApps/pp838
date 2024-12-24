using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle
{
    public class CellHolder : MonoBehaviour
    {
        [SerializeField] private List<Cell> _cells;
        [SerializeField] private int _gridWidth;
        [SerializeField] private int _gridHeight = 3;

        [SerializeField] private int _whiteCellIndex = 9;
        [SerializeField] private Vector2Int _whiteCellCoordinates;
        [SerializeField] private Vector2Int _stuckCoordinate;
        [SerializeField] private int _requiredLinesToComplete = 3;

        private Vector2Int _initWhiteCellCoordinates;
        private int _completedLinesCount;

        private HashSet<int> _completedRows = new HashSet<int>();
        private HashSet<int> _completedCols = new HashSet<int>();

        public event Action AllLinesCompleted;

        private void OnEnable()
        {
            foreach (var cell in _cells)
            {
                cell.Clicked += TrySwapCells;
            }

            _initWhiteCellCoordinates = _whiteCellCoordinates;
            _completedLinesCount = 0;
        }

        private void OnDisable()
        {
            foreach (var cell in _cells)
            {
                cell.Clicked -= TrySwapCells;
            }
        }

        public void ShuffleAllElementColors()
        {
            List<ColorTypes> colorTypes = new List<ColorTypes>();
            foreach (var cell in _cells)
            {
                if (cell.ColorType != ColorTypes.White && cell.ColorType != ColorTypes.Empty)
                    colorTypes.Add(cell.ColorType);
            }

            for (int i = 0; i < colorTypes.Count; i++)
            {
                int randomIndex = Random.Range(0, colorTypes.Count);
                (colorTypes[i], colorTypes[randomIndex]) = (colorTypes[randomIndex], colorTypes[i]);
            }

            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i].ColorType != ColorTypes.White && _cells[i].ColorType != ColorTypes.Empty)
                    _cells[i].SetColorType(colorTypes[i]);
            }
        }

        private Vector2Int GetCellCoordinates(int index)
        {
            if (index == _whiteCellIndex)
            {
                return _whiteCellCoordinates;
            }

            return new Vector2Int(index / _gridWidth, index % _gridWidth);
        }

        private bool AreCellsAdjacent(Cell cellA, Cell cellB)
        {
            int indexA = _cells.IndexOf(cellA);
            int indexB = _cells.IndexOf(cellB);

            if (indexA == -1 || indexB == -1)
                return false;

            Vector2Int coordA = GetCellCoordinates(indexA);
            Debug.Log(coordA);
            Vector2Int coordB = GetCellCoordinates(indexB);
            Debug.Log(coordB);

            if (coordA == _stuckCoordinate)
            {
                coordA = _initWhiteCellCoordinates;
            }

            if (coordB == _stuckCoordinate)
            {
                coordB = _initWhiteCellCoordinates;
            }

            return Mathf.Abs(coordA.x - coordB.x) + Mathf.Abs(coordA.y - coordB.y) == 1;
        }

        private void TrySwapCells(Cell clickedCell)
        {
            Cell whiteCell = _cells[_whiteCellIndex];

            if (AreCellsAdjacent(clickedCell, whiteCell))
            {
                ColorTypes tempColor = clickedCell.ColorType;
                clickedCell.SetColorType(whiteCell.ColorType);
                whiteCell.SetColorType(tempColor);

                int clickedIndex = _cells.IndexOf(clickedCell);
                var coord = GetCellCoordinates(clickedIndex);
                Debug.Log(coord);
                _whiteCellIndex = clickedIndex;
                Debug.Log(clickedIndex);
                _whiteCellCoordinates = coord;
                CheckForCompletedLines();
            }
            else
            {
                Debug.Log("Cells are not adjacent. Swap not possible.");
            }
        }

        private void CheckForCompletedLines()
        {
            // Check horizontal lines
            for (int row = 0; row < _gridHeight; row++)
            {
                // Skip rows already counted as completed
                if (_completedRows.Contains(row))
                    continue;

                ColorTypes rowColor = _cells[row * _gridWidth].ColorType;

                // Skip rows with invalid colors
                if (rowColor == ColorTypes.White || rowColor == ColorTypes.Empty)
                    continue;

                if (CheckHorizontalLine(row, rowColor))
                {
                    Debug.Log($"Horizontal line completed at row {row} with color {rowColor}");
                    _completedRows.Add(row);
                    _completedLinesCount++;
                }
            }

            // Check vertical lines
            for (int col = 0; col < _gridWidth; col++)
            {
                // Skip columns already counted as completed
                if (_completedCols.Contains(col))
                    continue;

                ColorTypes colColor = _cells[col].ColorType;

                // Skip columns with invalid colors
                if (colColor == ColorTypes.White || colColor == ColorTypes.Empty)
                    continue;

                if (CheckVerticalLine(col, colColor))
                {
                    Debug.Log($"Vertical line completed at column {col} with color {colColor}");
                    _completedCols.Add(col);
                    _completedLinesCount++;
                }
            }

            // Trigger the event if all required lines are completed
            if (_completedLinesCount >= _requiredLinesToComplete)
            {
                Debug.Log("All required lines completed!");
                AllLinesCompleted?.Invoke();
            }
        }

        private bool CheckHorizontalLine(int rowIndex, ColorTypes targetColor)
        {
            for (int col = 0; col < _gridWidth; col++)
            {
                int cellIndex = rowIndex * _gridWidth + col;
                if (_cells[cellIndex].ColorType != targetColor)
                    return false;
            }

            return true;
        }

        private bool CheckVerticalLine(int colIndex, ColorTypes targetColor)
        {
            for (int row = 0; row < _gridHeight; row++)
            {
                int cellIndex = row * _gridWidth + colIndex;
                if (_cells[cellIndex].ColorType != targetColor)
                    return false;
            }

            return true;
        }
    }
}