using _Game.Scripts.Classes;
using _Game.Scripts.Utility.EventBusSystem.Interfaces;

namespace _Game.Scripts.EventBusGameEvents
{
    public class PickUpWeapon : IEvent
    {
        public readonly Weapon _weapon;

        public PickUpWeapon(Weapon weapon)
        {
            _weapon = weapon;
        }
    }
}