using System.Linq;
using States;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class Desire_RepairHouse : Desire
    {
        public Desire_RepairHouse()
        {
            State = State_RepairHouse.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {
            var bias = villager.RepairHouseBias;


            var factorOne = villager.homes.Average(home => 1 - home.HouseHealth / home.MaxHouseHealth);


            DesireVal = bias * factorOne;

            villager.RepairHouseBias = DesireVal;

        }
    }
}