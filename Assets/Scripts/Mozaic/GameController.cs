using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mozaic
{
    public class GameController : MonoBehaviour
    {
        private const int StartDifficulty = 2;
        private const int StartTimerValue = 10;
        private const int MaxDifficulty = 9;

        [Header("UI Elements")]
        [SerializeField] private TMP_Text _countdownText;
        [SerializeField] private TMP_Text _winText;
        [SerializeField] private TMP_Text _loseText;
        [SerializeField] private TMP_Text _totalText;
        [SerializeField] private TMP_Text _repeatText;

        [Header("Game Objects")]
        [SerializeField] private GameObject _startGameObjects;
        [SerializeField] private GameObject _chooseTypePlane;
        [SerializeField] private GameObject _chooseColorPlane;
        [SerializeField] private GameObject _ingameElements;

        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _highlightButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _verifyButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _againButton;
        [SerializeField] private Button _quitButton;

        [Header("Game Data")]
        [SerializeField] private List<Item> _items;
        [SerializeField] private List<ChooseFigureItem> _chooseFigureItems;
        [SerializeField] private List<ItemTypeHolder> _itemTypeHolders;
        [SerializeField] private List<ItemColorHolder> _itemColorHolders;
        [SerializeField] private ItemDataHolder _itemDataHolder;

        private int _currentDifficulty;
        private int _totalScore;
        private IEnumerator _countdownCoroutine;
        private ChooseFigureItem _currentChoseItem;

        private void OnEnable()
        {
            foreach (var type in _itemTypeHolders)
            {
                type.TypeSelected += AssignType;
            }

            foreach (var color in _itemColorHolders)
            {
                color.ColorSelected += AssignColor;
            }

            _verifyButton.onClick.AddListener(OnVerifyClicked);
            _againButton.onClick.AddListener(ShowStartScreen);
            _nextButton.onClick.AddListener(ShowStartScreen);
            _backButton.onClick.AddListener(BackClicked);
            _quitButton.onClick.AddListener(QuitGame);
            _highlightButton.onClick.AddListener(HighlightEmptyElement);
            _playButton.onClick.AddListener(StartGame);
        }

        private void OnDisable()
        {
            foreach (var type in _itemTypeHolders)
            {
                type.TypeSelected -= AssignType;
            }

            foreach (var color in _itemColorHolders)
            {
                color.ColorSelected -= AssignColor;
            }

            _verifyButton.onClick.RemoveListener(OnVerifyClicked);
            _againButton.onClick.RemoveListener(ShowStartScreen);
            _nextButton.onClick.RemoveListener(ShowStartScreen);
            _backButton.onClick.RemoveListener(BackClicked);
            _quitButton.onClick.RemoveListener(QuitGame);
            _highlightButton.onClick.RemoveListener(HighlightEmptyElement);
            _playButton.onClick.RemoveListener(StartGame);
        }

        private void Start()
        {
            _currentDifficulty = StartDifficulty;
            _totalScore = 0;
            ResetUIStates();
            ShowStartScreen();
            _chooseColorPlane.SetActive(false);
            _chooseTypePlane.SetActive(false);
        }

        private void ShowStartScreen()
        {
            ResetUIStates();
            _startGameObjects.SetActive(true);
            _ingameElements.SetActive(false);
            DisableAllItems();

            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
            }

            _countdownCoroutine = CountdownCoroutine();
            StartCoroutine(_countdownCoroutine);

            for (int i = 0; i < _currentDifficulty; i++)
            {
                if (i < _items.Count)
                {
                    _items[i].SetItemData(_itemDataHolder.GetRandomItem());
                    _items[i].SetColor(_itemDataHolder.GetColorByType(_items[i].ItemData.ColorType));
                    _items[i].gameObject.SetActive(true);
                }
            }
        }

        private IEnumerator CountdownCoroutine()
        {
            var timeValue = StartTimerValue;
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
            ResetUIStates();
            _startGameObjects.SetActive(false);
            _verifyButton.gameObject.SetActive(true);
            VerifyOkButtonStatus();
            _repeatText.enabled = true;

            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
                _countdownCoroutine = null;
            }

            for (int i = 0; i < _currentDifficulty; i++)
            {
                if (i < _chooseFigureItems.Count)
                {
                    _chooseFigureItems[i].gameObject.SetActive(true);
                    _items[i].gameObject.SetActive(false);
                    _chooseFigureItems[i].ButtonClicked += OnChooseFigureClicked;
                }
            }

            _ingameElements.SetActive(true);
            _totalText.text = "Total: " + _totalScore.ToString();
        }

        private void OnChooseFigureClicked(ChooseFigureItem item)
        {
            _currentChoseItem = item;
            _chooseTypePlane.SetActive(true);
        }

        private void AssignType(FigureType type)
        {
            if (_currentChoseItem == null) return;

            _currentChoseItem.SetFigure(type);
            _chooseTypePlane.SetActive(false);
            _chooseColorPlane.SetActive(true);
            
            foreach (var color in _itemColorHolders)
            {
                color.SetItemData(_currentChoseItem.FigureType);
            }
        }

        private void AssignColor(ColorType colorType)
        {
            if (_currentChoseItem == null) return;
            
            _currentChoseItem.SetColor(colorType);
            _chooseColorPlane.SetActive(false);
            VerifyOkButtonStatus();
        }

        private void VerifyOkButtonStatus()
        {
            var elementsSelected = _chooseFigureItems.Take(_currentDifficulty);
            var status = elementsSelected.All(item => item.isActiveAndEnabled && item.ImageSelected());
            _verifyButton.interactable = status;
        }

        private void HighlightEmptyElement()
        {
            var availableElement = _chooseFigureItems.FirstOrDefault(item => item.isActiveAndEnabled && !item.ImageSelected());

            if (availableElement != null)
            {
                var index = _chooseFigureItems.IndexOf(availableElement);
                availableElement.SetCorrectData(_items[index].ItemData);
            }
        }

        private void OnVerifyClicked()
        {
            int correctCount = 0;
            _repeatText.enabled = false;

            for (int i = 0; i < _currentDifficulty; i++)
            {
                if (i < _chooseFigureItems.Count &&
                    _chooseFigureItems[i].isActiveAndEnabled &&
                    _chooseFigureItems[i].VerifyResult(_items[i].ItemData))
                {
                    correctCount++;
                }
            }

            if (correctCount == _currentDifficulty)
            {
                ProcessGameWin();
            }
            else
            {
                ProcessGameLost(correctCount);
            }
        }

        private void ProcessGameWin()
        {
            _winText.enabled = true;
            _winText.text = $"Great<br>{_currentDifficulty} out of {_currentDifficulty} correct";
            _totalScore++;
            _totalText.text = "Total: " + _totalScore.ToString();

            if (_currentDifficulty < MaxDifficulty)
            {
                _currentDifficulty++;
            }

            _verifyButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(true);
        }

        private void ProcessGameLost(int correctCount)
        {
            _loseText.enabled = true;
            _loseText.text = $"Fail<br>{correctCount} out of {_currentDifficulty} correct";

            _verifyButton.gameObject.SetActive(false);
            _againButton.gameObject.SetActive(true);
        }

        private void DisableAllItems()
        {
            foreach (var item in _items)
            {
                item.gameObject.SetActive(false);
            }

            foreach (var chooseItem in _chooseFigureItems)
            {
                chooseItem.ButtonClicked -= OnChooseFigureClicked;
                chooseItem.gameObject.SetActive(false);
            }
        }

        private void BackClicked()
        {
            ShowStartScreen();
        }

        private void QuitGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        private void ResetUIStates()
        {
            _winText.enabled = false;
            _loseText.enabled = false;
            _repeatText.enabled = false;
            _nextButton.gameObject.SetActive(false);
            _againButton.gameObject.SetActive(false);
        }
    }
}
