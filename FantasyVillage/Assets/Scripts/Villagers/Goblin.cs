using Assets.Scripts.FiniteStateMachine;
using UnityEngine;

namespace Assets.Scripts.Villagers
{
    public class Goblin : MonoBehaviour
    {

        private StateMachine<Goblin> FSM;
        public Goblin(StateMachine<Goblin> fSM)
        {
            FSM = fSM;
        }
        private int Health = 100;
        private int Damage = 10;
        private float MoveSpeed = 10f;
        private float AttackCooldown = 3f;

    }


}