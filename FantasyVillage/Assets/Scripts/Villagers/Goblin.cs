using Assets.Scripts.FiniteStateMachine;
using UnityEngine;

namespace Villagers
{
    public class Goblin : Humanoid
    {

        private StateMachine<Goblin> _fsm;
        public Goblin(StateMachine<Goblin> fSm)
        {
            _fsm = fSm;
            MoveSpeed = Random.Range(3, 7);
        }
        private int health = 100;
        private int damage = 10;
        
        private float attackCooldown = 3f;

    }


}