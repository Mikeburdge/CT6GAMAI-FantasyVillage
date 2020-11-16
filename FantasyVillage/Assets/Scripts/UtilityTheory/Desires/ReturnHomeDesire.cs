using Assets.Scripts.Villagers;
using States;
using UtilityTheory;

namespace Desires
{
    public class ReturnHomeDesire : Desire
    {
        public ReturnHomeDesire()
        {
            state = GoHomeAndSleep.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {

            float bias = villager.ReturnHomeBias;

            float factorOne = 1 - (villager.Health / villager.MaxHealth);

            float factorTwo = 1 - (villager.Stamina / villager.MaxStamina);

            desireVal = bias * (factorOne + factorTwo);
        }
    }
}