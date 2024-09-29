using System;
using Controllers;
using EventBusGameEvents;
using Interfaces;
using NaughtyAttributes;
using R3;
using UnityEngine;
using Utility.EventBusSystem.EventBus;
using View;

namespace MainMechanics
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private HealthController _controller;
        [SerializeField] private HealthBarView _healthBarView;

        [SerializeField] private int _maxHealth;

        [ShowNonSerializedField] private int CurrentHealth;
        
        private int _currentValue
        {
            get => CurrentHealth;
            set
            {
                if (value < 0)
                    throw new Exception($"Damage {value} < 0");
                
                CurrentHealth = value;
                
                if (CurrentHealth > _maxHealth)
                    CurrentHealth = _maxHealth;
                
                _playerHealthReactiveProperty.Value = CurrentHealth;
                
                if (CurrentHealth <= 0)
                {
                    EventBus.Dispatch(new PlayerDead());
                }
            }
        }

        private readonly ReactiveProperty<int> _playerHealthReactiveProperty = new();
        private ReadOnlyReactiveProperty<int> PlayerHealthReactiveProperty => _playerHealthReactiveProperty;
        
        private void OnEnable()
        {
            SetCurrentHealth();
            
            _controller.Init(PlayerHealthReactiveProperty, _healthBarView);
        }

        private void SetCurrentHealth()
        {
            _currentValue = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentValue -= damage;
        }
    }
}
