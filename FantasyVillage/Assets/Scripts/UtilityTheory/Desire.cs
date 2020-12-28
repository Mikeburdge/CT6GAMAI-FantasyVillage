using Assets.Scripts.FiniteStateMachine;
using States;
using Villagers;

namespace UtilityTheory
{
    public class Desire
    {

        public float DesireVal;

        public State<Villager> State = State_Default.Instance;


        // Overidden by children
        public virtual void CalculateDesireValue(Villager villager) { }

    }
}