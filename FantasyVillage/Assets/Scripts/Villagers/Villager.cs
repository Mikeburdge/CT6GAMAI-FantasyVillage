using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.FiniteStateMachine;
using Desires;
using LocationThings;
using Priority_Queue;
using States;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UtilityTheory;
using static BehaviourTrees.CuttingTreeNodes;
using static BehaviourTrees.GenericNodes;
using static BehaviourTrees.HomeNodes;

namespace Assets.Scripts.Villagers
{
    [RequireComponent(typeof(VillagerBB))]
    public class Villager : MonoBehaviour
    {
        //Pop Up Menu

        public GameObject AIPopUp;

        private TextMeshProUGUI textMesh;


        private StateMachine<Villager> FSM;

        private SimplePriorityQueue<Desire> priorityQueue = new SimplePriorityQueue<Desire>();

        public Desire ReturnHomeDesire;
        public Desire BeginGatheringDesire;
        public Desire BeginIdleDesire;


        public Villager(StateMachine<Villager> fSM)
        {
            FSM = fSM;
        }

        #region Variables


        [SerializeField]
        private int health;

        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private int stamina;

        [SerializeField]
        private int maxStamina;

        [SerializeField]
        private GameObject home;

        [SerializeField]
        protected int Damage;

        [SerializeField]
        public float MoveSpeed;

        [SerializeField]
        protected float AttackCooldown;

        //Combat Skill: Determines the this villagers Capabilities in Combat
        [SerializeField]
        protected int CombatSpeed;

        //Construction Skill: Determines the speed in which this villager can construct or repair buildings
        [SerializeField]
        protected int ConstructionSpeed;

        //Farming Skill: Determines speed in which this villager can farm food
        [SerializeField]
        protected int FarmingSpeed;

        //Gathering Skill: Determines the speed in which this villager gathers wood and rocks from trees and bigger rocks
        [SerializeField]
        private int gatheringSpeed;

        protected bool isFemale;

        public NavMeshAgent navMesh;

        [SerializeField]
        private float returnHomeBias;

        [SerializeField]
        private float startGatheringBias;

        [SerializeField]
        private float idleBias; //TODO: MAKE THIS RANDOM BETWEEN LIKE 0.1 AND 2 OR SOMETHING SO SOME VILLAGERS ARE LAZIER THAN OTHERS


        /// 
        /// Getters and Setters
        /// 
        public float ReturnHomeBias { get => returnHomeBias; set => returnHomeBias = value; }
        public float StartGatheringBias { get => startGatheringBias; set => startGatheringBias = value; }
        public int Health { get => health; set => health = value; }
        public int GatheringSpeed { get => gatheringSpeed; set => gatheringSpeed = value; }
        public GameObject Home { get => home; set => home = value; }
        public int Stamina { get => stamina; set => stamina = value; }
        public float IdleBias { get => idleBias; set => idleBias = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public int MaxStamina { get => maxStamina; set => maxStamina = value; }


        //Behaviour Trees
        public BTNode BTRootNode;

        //Get reference to Villager Blackboard
        public VillagerBB bb;
        //Chop Trees Sequence Root
        public CompositeNode ChopTreeSequenceRoot;
        //Go Home Sequence Root
        public GoHomeDecorator GoHomeDecoratorRoot;
        //Idle Sequence Root
        public CompositeNode IdleSequenceRoot;


        #endregion



        public void Awake()
        {
            Instantiate(AIPopUp, transform);
            textMesh = AIPopUp.GetComponentInChildren<TextMeshProUGUI>();

            MaxHealth = 100;
            MaxStamina = 200;
            InitVariables();
            navMesh = GetComponent<NavMeshAgent>();
            FSM = new StateMachine<Villager>();
            FSM.Configure(this, DefaultState.Instance);

            priorityQueue = new SimplePriorityQueue<Desire>();
            ReturnHomeDesire = new ReturnHomeDesire();
            BeginGatheringDesire = new StartGatheringDesire();
            BeginIdleDesire = new IdleDesire();

            priorityQueue.Enqueue(BeginGatheringDesire, 1.0f);
            priorityQueue.Enqueue(ReturnHomeDesire, 1.0f);
            priorityQueue.Enqueue(BeginIdleDesire, 1.0f);

            InvokeRepeating(nameof(UpdateStateChange), 0.1f, 0.1f);
        }

        protected virtual void InitVariables() { }

        public void ChangeState(State<Villager> S)
        {
            FSM.ChangeState(S);
        }


        void UpdateStateChange()
        {
            Debug.Log("Update State Change Updates Every 0.1 Seconds");

            foreach (Desire desire in priorityQueue)
            {
                desire.CalculateDesireValue(this);
                priorityQueue.UpdatePriority(desire, desire.desireVal);
            }

            State<Villager> potentialState = priorityQueue.Last().state;

            if (!FSM.CheckCurrentState(potentialState))
            {
                ChangeState(potentialState);
            }
        }

        private void Start()
        {
            bb = GetComponent<VillagerBB>();


            #region Chopping Tree Behaviour Tree

            ChopTreeSequenceRoot = new Sequence(bb);

            //Chop Tree Selector
            CompositeNode ChopTreeSelector = new Selector(bb);

            //Find and move to tree sequence Sequence
            CompositeNode FindAndMoveToTreeSequence = new Sequence(bb);

            //Chop Sequence Node
            CompositeNode ChopSequence = new Sequence(bb);

            //Chop Sequence Decorator Node
            ChopTreeDecorator ChopDecorator = new ChopTreeDecorator(ChopSequence, bb);

            //Find an available tree Decorator Node
            FindTreeDecorator FindTreeDecorator = new FindTreeDecorator(FindAndMoveToTreeSequence, bb);

            //Chop Tree Sequence

            ChopTreeSequenceRoot.AddChild(new GetMoveToLocation(bb, LocationNames.forest)); // gets the location to move towards
            ChopTreeSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination
            ChopTreeSequenceRoot.AddChild(new VillagerWaitTillAtLocation(bb, this)); // wait till we reached destination
            ChopTreeSequenceRoot.AddChild(ChopTreeSelector);

            //Chop Tree Selector

            ChopTreeSelector.AddChild(FindTreeDecorator);
            ChopTreeSelector.AddChild(ChopDecorator);


            //Find and move to tree sequence sequence

            FindAndMoveToTreeSequence.AddChild(new PickNearestTree(bb, this)); // pick the nearest tree to chop
            FindAndMoveToTreeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination
            FindAndMoveToTreeSequence.AddChild(new VillagerWaitTillAtLocation(bb, this)); // wait till we reached destination

            //CHOP Sequence

            ChopSequence.AddChild(new DelayNode(bb, GatheringSpeed, this)); // wait for 2 seconds
            ChopSequence.AddChild(new ChopTree(bb, this)); //chop tree while tree health is more than 0


            //SLIGHT ISSUE WHERE IT'LL START ADDING TO THE WOOD before the player reaches the tree but thats fine.
            #endregion

            #region Go Home

            CompositeNode GoHomeSequence = new Sequence(bb);

            CompositeNode RestSequence = new Sequence(bb);

            GoHomeDecoratorRoot = new GoHomeDecorator(GoHomeSequence, bb, this);

            GoHomeSequence.AddChild(new SetMoveToHome(bb, this)); //Set Home Location
            GoHomeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            GoHomeSequence.AddChild(new VillagerWaitTillAtLocation(bb, this)); // wait till we reached destination
            GoHomeSequence.AddChild(new EnterHome(bb, this)); // "enter the home"
            GoHomeSequence.AddChild(RestSequence);

            RestSequence.AddChild(new ReplenishHealthAndStamina(bb, this));
            RestSequence.AddChild(new DelayNode(bb, 2, this));

            #endregion

            #region Idle

            IdleSequenceRoot = new Sequence(bb);

            IdleSequenceRoot.AddChild(new PickRandomLocationNearby(bb, this)); //Set Home Location
            IdleSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            IdleSequenceRoot.AddChild(new VillagerWaitTillAtLocation(bb, this)); // wait till we reached destination
            IdleSequenceRoot.AddChild(new DelayNode(bb, 5, this));
            IdleSequenceRoot.AddChild(IdleSequenceRoot);
            #endregion

            ////Execute current BT every 0.1 seconds
            InvokeRepeating(nameof(UpdateFSM), 0.1f, 0.1f);
        }


        public void VillagerMoveTo(Vector3 MoveLocation)
        {
            navMesh.SetDestination(MoveLocation);
            navMesh.isStopped = false;
        }

        public void StopMovement()
        {
            navMesh.isStopped = true;
        }

        public void UpdateFSM()
        {
            FSM.Update();
        }

        public void ExecuteBT()
        {
            BTRootNode.Execute();
        }
        void ChangeBehaviourTree(BTNode RootNode)
        {
            BTRootNode = RootNode;
        }

        public void StartChoppingTreesBT()
        {
            ChangeBehaviourTree(ChopTreeSequenceRoot);
        }

        public void StartGoHomeAndSleepBT()
        {
            ChangeBehaviourTree(GoHomeDecoratorRoot);
        }

        public void StartIdleBT()
        {
            ChangeBehaviourTree(IdleSequenceRoot);
        }

        public void UpdateAIText(object message)
        {
            if (!textMesh)
            {
                Debug.Log("textMesh is invalid");
                return;
            }
            textMesh.SetText(message.ToString());
        }

        public void AppendAIText(object message)
        {
            if (!textMesh)
            {
                Debug.Log("textMesh is invalid");
                return;
            }
            textMesh.SetText(textMesh.text + "\n" + message);
        }

    }
}

