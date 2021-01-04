using States;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class Desire_Idle : Desire
    {
        public Desire_Idle()
        {
            State = State_Idle.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {

            var bias = villager.IdleBias;

            //I THINK FOR NOW THE IDLE BIAS CAN BE A SIMPLE HARD CODED NUMBER PER CLASS, MAYBE LATER IT CAN BE RANDOMISED TO GIVE A BIT OF LIFE

            DesireVal = bias;

            villager.IdleDesireValue = DesireVal;
        }
    }
}