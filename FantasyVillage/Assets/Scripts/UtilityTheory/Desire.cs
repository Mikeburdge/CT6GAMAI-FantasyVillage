using UnityEngine;
using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using Assets.Scripts.FiniteStateMachine.States;
using System;

namespace Assets.Scripts.UtilityTheory
{
    public class Desire
    {

        public float desireVal;

        public State<Villager> state = DefaultState.Instance;


        // Overidden by children
        public virtual void CalculateDesireValue(Villager villager) { }

    }
}