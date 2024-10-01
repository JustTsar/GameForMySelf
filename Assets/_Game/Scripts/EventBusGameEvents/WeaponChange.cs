using _Game.Scripts.Classes;
using _Game.Scripts.Utility.EventBusSystem.Interfaces;

namespace _Game.Scripts.EventBusGameEvents
{
    public class WeaponChange : IEvent
    {
        public readonly Weapon _weapon;

        public WeaponChange(Weapon weapon)
        {
            _weapon = weapon;
        }
    }
}