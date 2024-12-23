using System.Collections.Generic;
using UnityEngine;

namespace Memory
{
    public class CellTypeProvider
    {
        private readonly CellTypes[] _allTypes = (CellTypes[])System.Enum.GetValues(typeof(CellTypes));
    
        public List<CellTypes> GetPair(int pairsCount)
        {
            List<CellTypes> uniqueTypes = new List<CellTypes>();
            List<CellTypes> pairs = new List<CellTypes>(pairsCount * 2); 
        
            foreach (CellTypes cellType in _allTypes)
            {
                if (cellType != CellTypes.None)
                {
                    uniqueTypes.Add(cellType);
                }
            }
        
            while (pairs.Count < pairsCount * 2)
            {
                CellTypes randomCell = uniqueTypes[Random.Range(0, uniqueTypes.Count)];
                pairs.Add(randomCell);
                pairs.Add(randomCell);
            }
        
        
            ShuffleList(pairs);

            return pairs;
        }

        private void ShuffleList(List<CellTypes> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }

    public enum CellTypes
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        None
    }
}