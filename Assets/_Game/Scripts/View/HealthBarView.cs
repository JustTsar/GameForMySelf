using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _playerHealthBar;
        [SerializeField] private float _timeToChangeHealth = 2f;

        public void UpdateView(int value)
        {
            Debug.Log($"Take damage {value} {value/100}");
            
            DOTween.To(() => _playerHealthBar.value,
                x => _playerHealthBar.value = x,
                value / 100f,
                _timeToChangeHealth);
        }
    }
}