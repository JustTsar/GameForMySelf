using Enums;
using MainMechanics.Boosters;
using Utility.EventBusSystem.Interfaces;

namespace EventBusGameEvents
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