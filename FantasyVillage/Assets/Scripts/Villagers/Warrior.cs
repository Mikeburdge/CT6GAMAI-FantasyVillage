﻿using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace Assets.Scripts.Villagers
{
    public class Warrior : Villager
    {
        public Warrior(StateMachine<Villager> fsm) : base(fsm)
        {
        }
    }
}