using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace React
{
    public class GameController : MonoBehaviour
    {
        private const int MaxCountdownValue = 6;

        [SerializeField] private Sprite _redSprite;
        [SerializeField] private Sprite _greenSprite;

        [SerializeField] private GameObject _topPlane;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _gameButton;
        [SerializeField] private TMP_Text _currentResultText;
        [SerializeField] private LastResultHolder _lastResultHolder;
        [SerializeField] private ReactScoreDataHolder _reactScoreData;
        [SerializeField] private ResultsPlane _resultsPlane;

        private IEnumerator _countdownCoroutine;

        private float _timeBetweenClick;
        private float _greenTime;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartGame);
            _gameButton.onClick.AddListener(OnGameButtonClicked);
            _exitButton.onClick.AddListener(ShowStartScreen);
            _closeButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _gameButton.onClick.RemoveListener(OnGameButtonClicked);
            _exitButton.onClick.RemoveListener(ShowStartScreen);
            _closeButton.onClick.RemoveListener(QuitGame);
        }

        private void Start()
        {
            ShowStartScreen();
        }

        private void ShowStartScreen()
        {
            _topPlane.SetActive(true);
            _gameButton.gameObject.SetActive(false);
            _resultsPlane.gameObject.SetActive(true);
            _resultsPlane.DisplayTopResults();
            _exitButton.gameObject.SetActive(false);
            _currentResultText.enabled = false;
            _startButton.gameObject.SetActive(true);
            _lastResultHolder.gameObject.SetActive(false);
        }

        private void StartGame()
        {
            _topPlane.SetActive(false);
            _gameButton.gameObject.SetActive(true);
            _gameButton.image.sprite = _redSprite;
            _gameButton.enabled = false;
            _startButton.gameObject.SetActive(false);
            _resultsPlane.gameObject.SetActive(false);
            _exitButton.gameObject.SetActive(true);
            _lastResultHolder.gameObject.SetActive(true);

            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
            }

            _countdownCoroutine = ButtonActivationCoroutine();
            StartCoroutine(_countdownCoroutine);
        }

        private IEnumerator ButtonActivationCoroutine()
        {
            var randomActivationTime = Random.Range(0, MaxCountdownValue);
            Debug.Log(randomActivationTime);

            float timer = 0;

            while (timer < randomActivationTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            _gameButton.image.sprite = _greenSprite;
            _gameButton.enabled = true;
            
            _greenTime = Time.time;
        }

        private void OnGameButtonClicked()
        {
            if (_gameButton.image.sprite == _greenSprite)
            {
                _timeBetweenClick = Time.time - _greenTime;
                
                _currentResultText.enabled = true;
                _currentResultText.text = $"{_timeBetweenClick:F2} sec";
                
                _lastResultHolder.ActivateResult(_timeBetweenClick);
                _reactScoreData.AddScore(_timeBetweenClick);

                ResetGameButton();
            }
            else
            {
                Debug.LogWarning("You clicked too early!");
            }
        }

        private void ResetGameButton()
        {
            _gameButton.image.sprite = _redSprite;
            _gameButton.enabled = false;
            
            StartGame();
        }

        private void QuitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
