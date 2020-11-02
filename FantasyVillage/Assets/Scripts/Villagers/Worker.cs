using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.UtilityTheory.Desires;
using Assets.Scripts.Villagers;

public class Worker : Villager
{
    public Worker(StateMachine<Villager> fSM) : base(fSM)
    {
        Health = 60;
        Damage = 6;
        MoveSpeed = 0.12f;
        AttackCooldown = 2;

        ReturnHomeBias = 0.1f;
        StartFarmingBias = 0.3f;
    }


}
