using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.FiniteStateMachine.States;
using Assets.Scripts.UtilityTheory;
using Assets.Scripts.UtilityTheory.Desires;
using Priority_Queue;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Villagers
{
    public class Villager : MonoBehaviour
    {
        private StateMachine<Villager> FSM;

        private SimplePriorityQueue<Desire> priorityQueue = new SimplePriorityQueue<Desire>();

        public Desire ReturnHomeDesire;
        public Desire BeginFarmingDesire;


        public Villager(StateMachine<Villager> fSM)
        {
            FSM = fSM;
        }
        public enum Locations
        {
            home,
            forest,
            rockMine,
            farm,
            trainingArea,
            foodStorage
        }

        #region Variables

        public Locations Location = Locations.home;

        //Health is out of 100
        private int health;

        protected int Damage;

        public float MoveSpeed;

        protected float AttackCooldown;

        //Combat Skill: Determines the this villagers Capabilities in Combat
        protected int CombatSkill;

        //Construction Skill: Determines the speed in which this villager can construct or repair buildings
        protected int ConstructionSkill;

        //Farming Skill: Determines speed in which this villager can farm food
        protected int FarmingSkill;

        //Gathering Skill: Determines the speed in which this villager gathers wood and rocks from trees and bigger rocks
        protected int GatheringSkill;

        protected bool isFemale;

        public GameObject home;

        public GameObject farm;

        public NavMeshAgent navMesh;

        public bool shouldExecute;

        private float returnHomeBias;

        private float startFarmingBias;

        public float ReturnHomeBias { get => returnHomeBias; set => returnHomeBias = value; }
        public float StartFarmingBias { get => startFarmingBias; set => startFarmingBias = value; }
        public int Health { get => health; set => health = value; }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == home)
            {
                shouldExecute = true;
            }
            else if (other.gameObject == farm)
            {
                shouldExecute = true;
            }
        }

        public void Awake()
        {
            InitVariables();
            navMesh = GetComponent<NavMeshAgent>();
            Debug.Log(name + " awakens...");
            FSM = new StateMachine<Villager>();
            FSM.Configure(this, DefaultState.Instance);

            priorityQueue = new SimplePriorityQueue<Desire>();
            ReturnHomeDesire = new ReturnHomeDesire();
            BeginFarmingDesire = new StartFarmingDesire();
            priorityQueue.Enqueue(BeginFarmingDesire, 1.0f);
            priorityQueue.Enqueue(ReturnHomeDesire, 2.0f);

            InvokeRepeating(nameof(UpdateStateChange), 1f, 1f);
        }

        protected virtual void InitVariables() { }

        public void ChangeState(State<Villager> S)
        {
            FSM.ChangeState(S);
        }

        public void Update()
        {
            if (shouldExecute)
                FSM.Update();
        }

        void UpdateStateChange()
        {
            Debug.Log("This Should Update Every Second");

            foreach (Desire desire in priorityQueue)
            {
                desire.CalculateDesireValue(this);
                priorityQueue.UpdatePriority(desire, desire.desireVal);
            }

            State<Villager> potentialState = priorityQueue.First().state;

            if (!FSM.CheckCurrentState(potentialState))
            {
                ChangeState(potentialState);
            }


        }

        public void ChangeLocation(Locations l)
        {
            Location = l;
        }
    }
}