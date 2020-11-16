using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using States;

namespace UtilityTheory
{
    public class Desire
    {

        public float desireVal;

        public State<Villager> state = DefaultState.Instance;


        // Overidden by children
        public virtual void CalculateDesireValue(Villager villager) { }

    }
}