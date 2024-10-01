using _Game.Scripts.Utility.EventBusSystem.Interfaces;
using MainMechanics.Boosters;

namespace _Game.Scripts.EventBusGameEvents
{
    public class PickUpBooster : IEvent
    {
        public readonly Booster Booster;

        public PickUpBooster(Booster booster)
        {
            Booster = booster;
        }
    }
}