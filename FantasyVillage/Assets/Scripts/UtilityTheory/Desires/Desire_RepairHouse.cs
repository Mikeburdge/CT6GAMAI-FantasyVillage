using States;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class Desire_ReturnHome : Desire
    {
        public Desire_ReturnHome()
        {
            State = State_GoHomeAndSleep.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {

            var bias = villager.ReturnHomeBias;

            var factorOne = 1 - villager.Health / villager.MaxHealth;

            var factorTwo = 1 - villager.Stamina / villager.MaxStamina;

            DesireVal = bias * (factorOne + factorTwo);
            villager.ReturnHomeDesireValue = DesireVal;

        }
    }
}