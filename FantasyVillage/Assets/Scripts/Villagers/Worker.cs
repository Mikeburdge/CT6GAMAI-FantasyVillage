using Assets.Scripts.FiniteStateMachine;
using UnityEngine;
using Villagers;

public class Worker : Villager
{
    public Worker(StateMachine<Villager> FSM) : base(FSM)
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

        IdleBias = Random.Range(0.01f, 0.04f); //makes some villagers lazier than others

        staminaLoss = Random.Range(0.1f, 0.1f); //makes some villagers fatter than others which uses more stamina to move lol
    }

}