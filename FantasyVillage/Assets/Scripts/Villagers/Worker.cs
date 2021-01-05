using Assets.Scripts.FiniteStateMachine;
using Villagers;

public class Worker : Villager
{
    public Worker(StateMachine<Villager> fSm) : base(fSm)
    {
    }
    protected override void InitVariables()
    {
        Health = 100;
        Stamina = 100;
        damage = 6;
        MoveSpeed = 3f;
        attackCooldown = 2;

        GatheringSpeed = 1;

        ReturnHomeBias = 0.1f;
        StartGatheringBias = 0.8f;
    }

}