using _Game.Scripts.EventBusGameEvents;
using MainMechanics.Boosters;
using UnityEngine;
using Utility.EventBusSystem.EventBus;

namespace _Game.Scripts.MainMechanics
{
    public class CollisionDetector : MonoBehaviour
    { 
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Booster booster))
            {
                EventBus.Dispatch(new PickUpBooster(booster));
            }
        }
    }
}