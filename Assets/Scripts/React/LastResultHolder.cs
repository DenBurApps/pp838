using System;
using TMPro;
using UnityEngine;

namespace React
{
    public class LastResultHolder : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _lastResults;

        private int _iterator;
        private bool _playedOnce;

        private void OnEnable()
        {
            DisableAll();
            _playedOnce = false;
        }

        private void DisableAll()
        {
            foreach (var result in _lastResults)
            {
                result.enabled = false;
            }

            _iterator = 0;
        }

        public void ActivateResult(float result)
        {
            if (_lastResults.Length == 0 || !_playedOnce)
            {
                _playedOnce = true;
                return;
            }

            _lastResults[_iterator].enabled = true;
            _lastResults[_iterator].text = result.ToString("F2");
            
            _iterator = (_iterator + 1) % _lastResults.Length;
        }
    }
}