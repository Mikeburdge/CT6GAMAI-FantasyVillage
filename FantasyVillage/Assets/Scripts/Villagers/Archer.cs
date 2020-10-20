﻿using Assets.Scripts.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Villagers
{
    public class Archer : Villager
    {
        public Archer(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}