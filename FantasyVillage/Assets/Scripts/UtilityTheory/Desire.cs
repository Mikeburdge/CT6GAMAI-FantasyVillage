using UnityEngine;
using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using Assets.Scripts.FiniteStateMachine.States;
using System;

namespace Assets.Scripts.UtilityTheory
{
    public class Desire : IComparable<Desire>
    {

        protected float desireVal;
        public State<Villager> state = DefaultState.Instance;


        // Overidden by children
        public virtual void CalculateDesireValue(Villager villager) { }

        public int CompareTo(Desire otherDesire)
        {
            if (otherDesire == null)
            {
                Debug.Log("otherDesire is Invalid");
                return 1;
            }

            if (otherDesire.desireVal == 0 || desireVal == 0)
            {
                Debug.Log("desireVal is 0");
                return 1;
            }

            if (desireVal < otherDesire.desireVal) return -1;

            return 1;
        }
    }
}