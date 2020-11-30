using States;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class IdleDesire : Desire
    {
        public IdleDesire()
        {
            State = IdleState.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {

            float bias = villager.IdleBias;

            //I THINK FOR NOW THE IDLE BIAS CAN BE A SIMPLE HARD CODED NUMBER PER CLASS, MAYBE LATER IT CAN BE RANDOMISED TO GIVE A BIT OF LIFE

            DesireVal = bias;
        }
    }
}