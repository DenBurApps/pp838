using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Memory
{
    public class GameController : MonoBehaviour
    {
        private const int InitTimerValue = 5;
        private const int MaxDifficulty = 5;
        private const string GameStartText = "Memorize as many pairs as you can ";
        private const string InGameText = "Find a pair for each card";

        [SerializeField] private List<CellHolder> _cellHolders;
        [SerializeField] private GreatScreen _greatScreen;
        [SerializeField] private CellSpriteProvider _cellSpriteProvider;
        [SerializeField] private TMP_Text _memoryText;
        [SerializeField] private TMP_Text _countdownText;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _highlightButton;
        [SerializeField] private Button _closeButton;

        private CellTypeProvider _cellTypeProvider;
        private IEnumerator _timerCoroutine;
        private Cell _firstCell;
        private Cell _secondCell;
        private CellHolder _currentCellHolder;
        private int _cellPairs;
        private int _currentDifficulty;

        private void Awake()
        {
            _cellTypeProvider = new CellTypeProvider();
        }

        private void OnEnable()
        {
            foreach (var cellHolder in _cellHolders)
            {
                cellHolder.SetSpriteProvider(_cellSpriteProvider);
                cellHolder.CellClicked += ProcessCellClicked;
            }

            _greatScreen.Disabled += ShowStartScreen;
            _playButton.onClick.AddListener(StartGame);
            _highlightButton.onClick.AddListener(HighlighCard);
            _closeButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            foreach (var cellHolder in _cellHolders)
            {
                cellHolder.CellClicked -= ProcessCellClicked;
            }

            _greatScreen.Disabled -= ShowStartScreen;
            _playButton.onClick.RemoveListener(StartGame);
            _highlightButton.onClick.RemoveListener(HighlighCard);
            _closeButton.onClick.RemoveListener(QuitGame);
        }

        private void Start()
        {
            _currentDifficulty = 1;
            ShowStartScreen();
        }

        private void ShowStartScreen()
        {
            _currentCellHolder = _cellHolders[_currentDifficulty - 1];
            _cellPairs = _currentCellHolder.Cells.Count / 2;

            _currentCellHolder.gameObject.SetActive(true);

            foreach (var cellHolder in _cellHolders)
            {
                if (cellHolder != _currentCellHolder)
                    cellHolder.gameObject.SetActive(false);
            }

            _memoryText.text = GameStartText;
            _playButton.gameObject.SetActive(true);
            _highlightButton.gameObject.SetActive(false);

            List<CellTypes> cellTypesList = _cellTypeProvider.GetPair(_cellPairs);

            for (int i = 0; i < cellTypesList.Count; i++)
            {
                _currentCellHolder.Cells[i].ReturnToDefault();
                _currentCellHolder.Cells[i].SetCellType(cellTypesList[i]);
            }

            _currentCellHolder.ShowAllCells();

            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            _timerCoroutine = CountdownCoroutine();
            StartCoroutine(_timerCoroutine);
        }

        private IEnumerator CountdownCoroutine()
        {
            var timeValue = InitTimerValue;
            _countdownText.enabled = true;
            _countdownText.text = timeValue.ToString();

            while (timeValue > 0)
            {
                yield return new WaitForSeconds(1);
                timeValue--;
                _countdownText.text = timeValue.ToString();
            }

            StartGame();
        }

        private void StartGame()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            _memoryText.text = InGameText;
            _countdownText.enabled = false;
            _currentCellHolder.HideAllCellsImages();
            _playButton.gameObject.SetActive(false);
            _highlightButton.gameObject.SetActive(true);
        }

        private void ProcessCellClicked(Cell cell)
        {
            if (cell.IsFliped) return;

            if (_firstCell != null && _secondCell != null)
                ResetSelectedCells();

            if (_firstCell == null)
            {
                _firstCell = cell;
                _firstCell.ShowCellImage();
            }
            else if (_firstCell != cell)
            {
                _secondCell = cell;
                _secondCell.ShowCellImage();
                CompareChosenCells();
            }
        }

        private void ResetSelectedCells()
        {
            _firstCell?.HideCellImage();
            _secondCell?.HideCellImage();
            _firstCell = null;
            _secondCell = null;
        }

        private void CompareChosenCells()
        {
            if (_firstCell.CurrentType == _secondCell.CurrentType)
            {
                _cellPairs--;

                _firstCell.Disable();
                _secondCell.Disable();

                if (_cellPairs <= 0)
                {
                    ProcessGameWon();
                }

                _firstCell = null;
                _secondCell = null;
            }
        }

        private void ProcessGameWon()
        {
            _greatScreen.gameObject.SetActive(true);

            if (_currentDifficulty + 1 < MaxDifficulty)
                _currentDifficulty++;
        }

        private void HighlighCard()
        {
            StartCoroutine(ShowCard());
        }

        private IEnumerator ShowCard()
        {
            var randomCard = _currentCellHolder.Cells.FirstOrDefault(cell => !cell.IsFliped);

            if (randomCard != null)
                randomCard.ShowCellImage();

            yield return new WaitForSeconds(2);

            if (randomCard != null)
                randomCard.HideCellImage();
        }

        private void QuitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}