using System.Collections.Generic;
using _Game.Scripts.Classes;
using UnityEngine;

namespace _Game.Scripts.View
{
    public class WeaponPanelView : MonoBehaviour
    {
        [SerializeField] private Transform _parentPanel;
        [SerializeField] private WeaponSlot _weaponSlotPrefab;

        private Weapon selectedWeapon;
        
        private readonly List<WeaponSlot> _weaponSlots = new();
        
        public void Init(List<Weapon> weaponViewData)
        {
            foreach (var data in weaponViewData)
            {
                WeaponSlot newWeaponSlot = Instantiate(_weaponSlotPrefab, _parentPanel);
                newWeaponSlot.Init(data);
                _weaponSlots.Add(newWeaponSlot);
            }
        }

        public void UpdateView(Weapon newWeapon)
        {
            selectedWeapon = newWeapon;
           _weaponSlots.ForEach((_) => UpdateView(selectedWeapon));
        }
    }
}