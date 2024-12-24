using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Puzzle
{
    public class GameController : MonoBehaviour
    {
        private const int InitDifficulty = 1;
        private const int MaxDifficulty = 4;

        [SerializeField] private List<CellHolder> _cellHolders;
        [SerializeField] private GreatScreen _greatScreen;
        [SerializeField] private TMP_Text _currentDifficultyText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _backButton;

        private int _currentDifficulty;
        private CellHolder _currentCellHolder;

        private void OnEnable()
        {
            _greatScreen.Disabled += StartGame;
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _closeButton.onClick.AddListener(QuitGame);

            foreach (var cellHolder in _cellHolders)
            {
                cellHolder.AllLinesCompleted += ProcessGameWin;
            }
        }

        private void OnDisable()
        {
            _greatScreen.Disabled -= StartGame;
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _closeButton.onClick.RemoveListener(QuitGame);

            foreach (var cellHolder in _cellHolders)
            {
                cellHolder.AllLinesCompleted -= ProcessGameWin;
            }
        }

        private void Start()
        {
            _currentDifficulty = InitDifficulty;
            UpdateDifficultyText();
            StartGame();
        }

        private void UpdateDifficultyText()
        {
            _currentDifficultyText.text = _currentDifficulty.ToString();
        }

        private void StartGame()
        {
            _currentCellHolder = _cellHolders[_currentDifficulty - 1];

            _currentCellHolder.gameObject.SetActive(true);
            _currentCellHolder.ShuffleAllElementColors();

            foreach (var cellHolder in _cellHolders)
            {
                if (cellHolder != _currentCellHolder)
                    cellHolder.gameObject.SetActive(false);
            }
        }

        private void ProcessGameWin()
        {
            if (_currentDifficulty + 1 < MaxDifficulty)
            {
                _currentDifficulty++;
                UpdateDifficultyText();
            }

            _greatScreen.gameObject.SetActive(true);
        }

        private void OnBackButtonClicked()
        {
            _currentDifficulty = Mathf.Clamp(_currentDifficulty - 1, InitDifficulty, MaxDifficulty);

            StartGame();
        }

        private void QuitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}