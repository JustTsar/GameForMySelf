using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Classes;
using _Game.Scripts.EventBusGameEvents;
using _Game.Scripts.Utility.EventBusSystem.Subscription;
using _Game.Scripts.View;
using UnityEngine;
using Utility.EventBusSystem.EventBus;

namespace _Game.Scripts.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponPanelView _weaponPanelView;
        [SerializeField] private List<Weapon> _weapons = new();
        
        private readonly Subscriptions _subscriptions = new();

        private void Start()
        {
            _weaponPanelView.Init(_weapons);
        }
        
        private void OnEnable()
        {
            _subscriptions.Add(EventBus.Subscribe<WeaponChange>(OnWeaponChange));
        }

        private void OnWeaponChange(WeaponChange arg)
        {
            _weaponPanelView.UpdateView(arg._weapon);
        }
    }
}