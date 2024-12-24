using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace React
{
    public class ReactScoreDataHolder : MonoBehaviour
    {
        private string _savePath;
        private List<float> _scores;
        private ReactScoreData _reactScoreData;

        public ReactScoreData ReactScoreData => _reactScoreData;
        
        private void Awake()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "ReactScore");
            _reactScoreData = new ReactScoreData();
            _scores = new List<float>();
            LoadStats();
        }

        private void SaveScores()
        {
            var json = JsonConvert.SerializeObject(_reactScoreData, Formatting.Indented);
            File.WriteAllText(_savePath, json);
            Debug.Log("saved");
        }

        private void LoadStats()
        {
            if (!File.Exists(_savePath))
            {
                Debug.Log("no saves");
            
                return;
            }

            var json = File.ReadAllText(_savePath);
            var data = JsonConvert.DeserializeObject<ReactScoreData>(json);

            _reactScoreData = data;
        
            Debug.Log("loaded");
        }
        
        public void AddScore(float score)
        {
            _scores.Add(score);
            
            UpdateMinimumAndMaximum(score);
            
            if (_scores.Count >= 5)
            {
                CalculateAndSaveAverage();
                _scores.Clear();
            }

            SaveScores();
        }

        private void UpdateMinimumAndMaximum(float score)
        {
            if (_reactScoreData.MinimumTimes.Count == 0 || score < _reactScoreData.MinimumTimes[^1])
            {
                _reactScoreData.MinimumTimes.Add(score);
                Debug.Log($"New minimum score added: {score:F2}");
            }

            if (_reactScoreData.MaximumTimes.Count == 0 || score > _reactScoreData.MaximumTimes[^1])
            {
                _reactScoreData.MaximumTimes.Add(score);
                Debug.Log($"New maximum score added: {score:F2}");
            }
            
            SaveScores();
        }

        private void CalculateAndSaveAverage()
        {
            float average = 0;

            foreach (var score in _scores)
            {
                average += score;
            }

            average /= _scores.Count;

            _reactScoreData.AverageTimes.Add(average);
            Debug.Log($"New average score added: {average:F2}");
        }
    }

    [Serializable]
    public class ReactScoreData
    {
        public List<float> AverageTimes = new();
        public List<float> MinimumTimes = new();
        public List<float> MaximumTimes = new();
    }
    
}
