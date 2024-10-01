using System;
using _Game.Scripts.ScriptableObjects;

namespace _Game.Scripts.Classes
{
    [Serializable]
    public class Weapon
    {
        private WeaponData _weaponData;
        private WeaponViewData _weaponViewData;
        private bool _isWeaponOpened;


        public WeaponData WeaponData => _weaponData;
        public WeaponViewData WeaponViewData => _weaponViewData;
        public bool IsWeaponOpened => _isWeaponOpened;

        public void OpenWeapon()
        {
            _isWeaponOpened = true;
        }
    }
}