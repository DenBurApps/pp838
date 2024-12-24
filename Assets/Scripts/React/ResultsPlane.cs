using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace React
{
    public class ResultsPlane : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _averageTexts;
        [SerializeField] private TMP_Text[] _minumumTexts;
        [SerializeField] private TMP_Text[] _maximumTexts;
        [SerializeField] private ReactScoreDataHolder _dataHolder;

        public void DisplayTopResults()
        {
            var averages = _dataHolder.ReactScoreData.AverageTimes.OrderBy(x => x).Take(3).ToList();
            var minimums = _dataHolder.ReactScoreData.MinimumTimes.OrderBy(x => x).Take(3).ToList();
            var maximums = _dataHolder.ReactScoreData.MaximumTimes.OrderBy(x => x).Take(3).ToList();

            UpdateTextArray(_averageTexts, averages);
            UpdateTextArray(_minumumTexts, minimums);
            UpdateTextArray(_maximumTexts, maximums);
        }

        private void UpdateTextArray(TMP_Text[] textArray, List<float> values)
        {
            for (int i = 0; i < textArray.Length; i++)
            {
                textArray[i].text = i < values.Count ? values[i].ToString("F2") : "-";
            }
        }
    }
}
