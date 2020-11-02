using Assets.Scripts.FiniteStateMachine;

namespace Assets.Scripts.Villagers
{
    public class Warrior : Villager
    {
        public Warrior(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}