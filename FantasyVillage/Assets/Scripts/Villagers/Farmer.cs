using Assets.Scripts.FiniteStateMachine;

namespace Assets.Scripts.Villagers
{
    public class Farmer : Villager
    {
        public Farmer(StateMachine<Villager> fSM) : base(fSM)
        {

        }

        protected override void InitVariables()
        {
            Health = 60;
            Damage = 6;
            MoveSpeed = 0.12f;
            AttackCooldown = 2;

            ReturnHomeBias = 0.1f;
            StartGatheringBias = 0.60f;
        }
    }
}