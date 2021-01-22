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
            State = StateRepairHouse.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {
            var bias = villager.RepairHouseBias;

            var lowestHealth = villager.homes[0];

            //Get House On Lowest Health
            foreach (var house in villager.homes.Where(house => house.HouseHealth < lowestHealth.HouseHealth))
            {
                lowestHealth = house;
            }
            
            var factorOne = 1 - (lowestHealth.HouseHealth / lowestHealth.MaxHouseHealth);


            DesireVal = bias * factorOne;

            villager.DisplayRepairHouseDesire = DesireVal;
        }
    }
}