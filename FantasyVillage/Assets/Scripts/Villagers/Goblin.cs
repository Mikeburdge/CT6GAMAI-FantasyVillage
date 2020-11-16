using Assets.Scripts.FiniteStateMachine;
using UnityEngine;

namespace Assets.Scripts.Villagers
{
    public class Goblin : MonoBehaviour
    {

        private StateMachine<Goblin> _fsm;
        public Goblin(StateMachine<Goblin> fSm)
        {
            _fsm = fSm;
        }
        private int _health = 100;
        private int _damage = 10;
        private float _moveSpeed = 10f;
        private float _attackCooldown = 3f;

    }


}