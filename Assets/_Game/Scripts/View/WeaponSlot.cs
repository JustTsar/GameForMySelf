using _Game.Scripts.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.View
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private Image _mainImage;
        [SerializeField] private Image _outLine;
        
        private Weapon _weapon;

        private void UpdateView(bool isSelected)
        {
            _outLine.gameObject.SetActive(isSelected);
            _mainImage.sprite = _weapon.IsWeaponOpened ? _weapon.WeaponViewData.OpenedIcon : _weapon.WeaponViewData.LockedIcon;
        }

        public void Init(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void UpdateState(Weapon weapon)
        {
            UpdateView(weapon == _weapon);
        }
    }
}