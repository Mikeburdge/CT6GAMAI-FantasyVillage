using Assets.Scripts.FiniteStateMachine;

namespace Assets.Scripts.Villagers
{
    public class Farmer : Villager
    {
        public Farmer(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}