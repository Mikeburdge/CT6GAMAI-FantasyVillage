using Assets.Scripts.FiniteStateMachine;

namespace Assets.Scripts.Villagers
{
    public class Wizard : Villager
    {
        public Wizard(StateMachine<Villager> fSM) : base(fSM)
        {
        }
    }
}