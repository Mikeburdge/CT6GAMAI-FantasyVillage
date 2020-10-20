using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.FiniteStateMachine.States;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Villagers
{
    public class Villager : MonoBehaviour
    {
        private StateMachine<Villager> FSM;

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
            foodStorage,
            travelling
        }

        public Locations Location = Locations.forest;
        protected int Health;
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



        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == home)
            {

            }
            else if (other.gameObject == farm)
            {

            }
        }

        public void Awake()
        {
            navMesh = GetComponent<NavMeshAgent>();
            Debug.Log(name + " awakes...");
            FSM = new StateMachine<Villager>();
            FSM.Configure(this, GoHomeAndSleep.Instance);
        }

        public void ChangeState(State<Villager> e)
        {
            FSM.ChangeState(e);
        }

        public void Update()
        {
            Debug.Log(name + " Updating...");
            FSM.Update();
        }
        public void ChangeLocation(Locations l)
        {
            Location = l;
        }




    }
}