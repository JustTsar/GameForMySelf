using System;
using R3;
using UnityEngine;
using View;

namespace Controllers
{
    public class HealthController : MonoBehaviour
    {
        private ReadOnlyReactiveProperty<int> _readOnlyReactiveProperty;
        private HealthBarView _healthBarView;

        private IDisposable _disposable;
        
        public void Init(ReadOnlyReactiveProperty<int> damageReactiveProperty, HealthBarView healthBarView)
        {
            _healthBarView = healthBarView;
            
            _readOnlyReactiveProperty = damageReactiveProperty;
            _disposable =  _readOnlyReactiveProperty.Subscribe(UpdateView);
        }

        private void UpdateView(int value)
        {
            _healthBarView.UpdateView(value);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }
    }
}