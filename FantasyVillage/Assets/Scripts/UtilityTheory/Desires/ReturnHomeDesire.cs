using States;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class ReturnHomeDesire : Desire
    {
        public ReturnHomeDesire()
        {
            State = GoHomeAndSleep.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {

            float bias = villager.ReturnHomeBias;

            float factorOne = 1 - (villager.Health / villager.MaxHealth);

            float factorTwo = 1 - (villager.Stamina / villager.MaxStamina);

            DesireVal = bias * (factorOne + factorTwo);
        }
    }
}