using Assets.Scripts.FiniteStateMachine;

namespace Assets.Scripts.Villagers
{
    public class Archer : Villager
    {
        public Archer(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}