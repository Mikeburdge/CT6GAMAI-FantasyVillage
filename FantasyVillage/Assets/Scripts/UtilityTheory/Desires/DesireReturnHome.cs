using States;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class DesireReturnHome : Desire
    {
        public DesireReturnHome()
        {
            State = StateGoHomeAndSleep.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {

            var bias = villager.ReturnHomeBias;

            var factorOne = 1 - villager.Health / villager.MaxHealth;

            var factorTwo = 1 - villager.Stamina / villager.MaxStamina;

            DesireVal = bias * (factorOne + factorTwo);

            villager.DisplayReturnHomeDesire = DesireVal;

        }
    }
}