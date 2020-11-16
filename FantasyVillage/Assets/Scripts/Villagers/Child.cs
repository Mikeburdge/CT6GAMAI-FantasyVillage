using Assets.Scripts.FiniteStateMachine;

namespace Assets.Scripts.Villagers
{
    public class Child : Villager
    {
        public Child(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}