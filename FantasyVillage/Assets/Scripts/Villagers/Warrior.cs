using Assets.Scripts.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Villagers
{
    public class Warrior : Villager
    {
        public Warrior(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}