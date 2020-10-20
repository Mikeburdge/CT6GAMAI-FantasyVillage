using Assets.Scripts.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Villagers
{
    public class Wizard : Villager
    {
        public Wizard(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}