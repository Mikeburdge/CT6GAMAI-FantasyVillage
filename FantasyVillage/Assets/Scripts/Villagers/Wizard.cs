using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace Assets.Scripts.Villagers
{
    public class Wizard : Villager
    {
        public Wizard(StateMachine<Villager> fSm) : base(fSm)
        {
        }
    }
}