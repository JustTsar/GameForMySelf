﻿using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.EventBusGameEvents;
using _Game.Scripts.Utility.EventBusSystem.Subscription;
using MainMechanics.Boosters;
using UnityEngine;
using Utility.EventBusSystem.EventBus;

namespace _Game.Scripts.Controllers
{
    public class BoosterController : MonoBehaviour
    {
        private readonly Dictionary<BoosterType, Booster> _activeBoosters = new();

        private readonly Subscriptions _subscriptions = new();

        private void OnBoosterPickUp(PickUpBooster arg)
        {
            if (HasActiveBoostByType(arg.Booster.BoosterType, out var booster))
            {
                booster.ReloadBoost();
            }
            else
            {
                AddBoost(arg.Booster.BoosterType ,arg.Booster);
            }
        }

        private bool HasActiveBoostByType(BoosterType boosterType, out Booster booster)
        {
            booster = _activeBoosters.GetValueOrDefault(boosterType);

            return booster != null;
        }

        private void AddBoost(BoosterType argBoosterType, Booster boosterType)
        {
            _activeBoosters.Add(argBoosterType, boosterType);
        }
        
        private void RemoveBoost(BoosterType argBoosterType)
        {
            _activeBoosters.Remove(argBoosterType);
        }

        private void OnEnable()
        {
            _subscriptions.Add(EventBus.Subscribe<PickUpBooster>(OnBoosterPickUp));
        }

        private void OnDisable()
        {
            _subscriptions.UnsubscribeAll();
        }
    }
}