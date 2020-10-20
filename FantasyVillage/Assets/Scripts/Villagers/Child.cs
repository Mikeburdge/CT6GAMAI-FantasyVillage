using Assets.Scripts.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Villagers
{
    public class Child : Villager
    {
        public Child(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}