using System;
using System.Collections;
using UnityEngine;

namespace Memory
{
    public class GreatScreen : MonoBehaviour
    {
        private int _countdown = 3;

        public event Action Disabled;

        private void OnEnable()
        {
            StartCoroutine(StartDisabling());
        }

        private IEnumerator StartDisabling()
        {
            WaitForSeconds interval = new WaitForSeconds(_countdown);

            yield return interval;
            
            Disabled?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
