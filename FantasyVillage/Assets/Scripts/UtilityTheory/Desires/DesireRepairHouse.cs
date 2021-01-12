using System.Linq;
using States;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class DesireRepairHouse : Desire
    {
        public DesireRepairHouse()
        {
            State = State_RepairHouse.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {
            var bias = villager.RepairHouseBias;


            var factorOne = villager.homes.Average(home => 1 - home.HouseHealth / home.MaxHouseHealth);


            DesireVal = bias * factorOne;

            villager.RepairHouseDesireValue = DesireVal;
        }
    }
}