using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;

public class Worker : Villager
{
    public Worker(StateMachine<Villager> fSM) : base(fSM)
    {
    }
    protected override void InitVariables()
    {
        Health = 60;
        Damage = 6;
        MoveSpeed = 0.12f;
        AttackCooldown = 2;

        GatheringSpeed = 1;

        ReturnHomeBias = 0.1f;
        StartGatheringBias = 0.8f;
        IdleBias = 0.1f;
    }

}