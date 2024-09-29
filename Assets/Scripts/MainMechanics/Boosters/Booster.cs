using Enums;
using UnityEngine;

namespace MainMechanics.Boosters
{
    public abstract class Booster : MonoBehaviour
    {
        [SerializeField] private BoosterType _boosterType;

        public BoosterType BoosterType => _boosterType;

        public abstract void ActivateBoost();
        public abstract void DeactivateBoost();
        public abstract void ReloadBoost();
        
    }
}