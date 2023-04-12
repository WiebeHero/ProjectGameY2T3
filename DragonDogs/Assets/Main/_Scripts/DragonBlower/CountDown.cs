using System.Collections;
using TMPro;
using UnityEngine;

namespace Main._Scripts.DragonBlower
{
    public class CountDown : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;
    
        private int _number;
        private int _duration;

        public delegate void TimerEvent();

        public event TimerEvent FinishedEvent;

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        public void StartCountDown(int pDuration)
        {
            StopAllCoroutines();
            _number = pDuration;
        
            StartCoroutine(OneDown());
        }
    
        private IEnumerator OneDown()
        {
            yield return new WaitForSeconds(1);

            _number -= 1;
            _textMeshProUGUI.text = _number.ToString();

            if (_number > 0) StartCoroutine(OneDown());
            else FinishedEvent?.Invoke();
        }
    
        private void Update()
        {
            _textMeshProUGUI.text = _number.ToString();
        }
    }
}
