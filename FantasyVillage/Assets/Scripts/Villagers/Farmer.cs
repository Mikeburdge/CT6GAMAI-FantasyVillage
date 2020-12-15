﻿using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace Assets.Scripts.Villagers
{
    public class Farmer : Villager
    {
        public Farmer(StateMachine<Villager> fSm) : base(fSm)
        {

        }

        protected override void InitVariables()
        {
            Health = 60;
            damage = 6;
            MoveSpeed = 5;
            attackCooldown = 2;

            ReturnHomeBias = 0.1f;
            StartGatheringBias = 0.60f;
        }
    }
}