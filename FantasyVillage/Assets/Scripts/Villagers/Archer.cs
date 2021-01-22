using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace Assets.Scripts.Villagers
{
    public class Archer : Villager
    {
        public Archer(StateMachine<Villager> fsm) : base(fsm)
        {
        }
    }
}