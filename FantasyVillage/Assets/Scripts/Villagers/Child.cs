﻿using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace Assets.Scripts.Villagers
{
    public class Child : Villager
    {
        public Child(StateMachine<Villager> fsm) : base(fsm)
        {
        }
    }
}