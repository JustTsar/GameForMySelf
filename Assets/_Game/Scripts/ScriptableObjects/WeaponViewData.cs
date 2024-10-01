using UnityEngine;

namespace _Game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponViewData", menuName = "477/NewWeaponViewData", order = -1)]
    public class WeaponViewData : ScriptableObject
    {
        [SerializeField] private Sprite _openedIcon;
        [SerializeField] private Sprite _lockedIcon;
        [SerializeField] private WeaponData _weaponData;
        
        
        public Sprite OpenedIcon => _openedIcon;
        public Sprite LockedIcon => _lockedIcon;
        public WeaponData WeaponData => _weaponData;
        
    }
}