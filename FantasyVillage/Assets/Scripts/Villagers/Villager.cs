using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.FiniteStateMachine.States;
using Assets.Scripts.UtilityTheory;
using Assets.Scripts.UtilityTheory.Desires;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Villagers
{
    public class Villager : MonoBehaviour
    {
        private StateMachine<Villager> FSM;

        

        IndexedPriorityQueue<Desire> priorityQueue = new IndexedPriorityQueue<Desire>(2);


        private Desire ReturnHomeDesire = new ReturnHomeDesire();
        private Desire BeginFarmingDesire = new StartFarmingDesire();


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
            navMesh = GetComponent<NavMeshAgent>();
            Debug.Log(name + " awakes...");
            FSM = new StateMachine<Villager>();
            FSM.Configure(this, DefaultState.Instance);

            priorityQueue.Insert(1, ReturnHomeDesire);
            priorityQueue.Insert(2, BeginFarmingDesire);

            InvokeRepeating("UpdateStateChange", 1f, 1f);
        }

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
            for (int i = 0; i < priorityQueue.Count; i++)
            {
                priorityQueue[i].CalculateDesireValue(this);
            }

            ChangeState(priorityQueue.Top().state);

            Debug.Log("This Should Update Every Second");

        }

        public void ChangeLocation(Locations l)
        {
            Location = l;
        }
    }
}