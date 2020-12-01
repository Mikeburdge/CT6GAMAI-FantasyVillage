using System.Linq;
using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.FiniteStateMachine;
using Desires;
using LocationThings;
using Priority_Queue;
using States;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UtilityTheory;
using static BehaviourTrees.CuttingTreeNodes;
using static BehaviourTrees.GenericNodes;
using static BehaviourTrees.HomeNodes;

namespace Villagers
{
    [RequireComponent(typeof(VillagerBB))]
    public class Villager : MonoBehaviour
    {
        #region TextMeshPro Stuff

        //Pop Up Menu

        public GameObject AIPopUp;

        private TextMeshProUGUI textMeshPro;
        private TextMesh _mTextMesh;

        private Transform _mTransform;
        private Transform _mFloatingTextTransform;
        private Transform _mCameraTransform;

        #endregion

        private StateMachine<Villager> _fsm;

        private SimplePriorityQueue<Desire> _priorityQueue = new SimplePriorityQueue<Desire>();

        public Desire ReturnHomeDesire;
        public Desire BeginGatheringDesire;
        public Desire BeginIdleDesire;


        public Villager(StateMachine<Villager> fSm)
        {
            _fsm = fSm;
        }

        #region Variables


        [SerializeField] private int health;

        [SerializeField] private int maxHealth;

        [SerializeField] private int stamina;

        [SerializeField] private int maxStamina;

        [SerializeField] private GameObject home;

        [FormerlySerializedAs("Damage")] [SerializeField]
        protected int damage;

        [FormerlySerializedAs("MoveSpeed")] [SerializeField]
        public float moveSpeed;

        [FormerlySerializedAs("AttackCooldown")] [SerializeField]
        protected float attackCooldown;

        //Combat Skill: Determines the this villagers Capabilities in Combat
        [FormerlySerializedAs("CombatSpeed")] [SerializeField]
        protected int combatSpeed;

        //Construction Skill: Determines the speed in which this villager can construct or repair buildings
        [FormerlySerializedAs("ConstructionSpeed")] [SerializeField]
        protected int constructionSpeed;

        //Farming Skill: Determines speed in which this villager can farm food
        [FormerlySerializedAs("FarmingSpeed")] [SerializeField]
        protected int farmingSpeed;

        //Gathering Skill: Determines the speed in which this villager gathers wood and rocks from trees and bigger rocks
        [SerializeField] private int gatheringSpeed;

        //public NavMeshAgent navMesh;

        [SerializeField] private float returnHomeBias;

        [SerializeField] private float startGatheringBias;

        [SerializeField]
        private float
            idleBias; //TODO: MAKE THIS RANDOM BETWEEN LIKE 0.1 AND 2 OR SOMETHING SO SOME VILLAGERS ARE LAZIER THAN OTHERS



        /// 
        /// Getters and Setters
        /// 
        public float ReturnHomeBias
        {
            get => returnHomeBias;
            set => returnHomeBias = value;
        }

        public float StartGatheringBias
        {
            get => startGatheringBias;
            set => startGatheringBias = value;
        }

        public int Health
        {
            get => health;
            set => health = value;
        }

        public int GatheringSpeed
        {
            get => gatheringSpeed;
            set => gatheringSpeed = value;
        }

        public GameObject Home
        {
            get => home;
            set => home = value;
        }

        public int Stamina
        {
            get => stamina;
            set => stamina = value;
        }

        public float IdleBias
        {
            get => idleBias;
            set => idleBias = value;
        }

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int MaxStamina
        {
            get => maxStamina;
            set => maxStamina = value;
        }


        //Behaviour Trees
        public BtNode BtRootNode;

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

            MaxHealth = 100;
            MaxStamina = 200;
            InitVariables();
            //navMesh = GetComponent<NavMeshAgent>();

            _fsm = new StateMachine<Villager>();
            _fsm.Configure(this, DefaultState.Instance);

            _priorityQueue = new SimplePriorityQueue<Desire>();

            ReturnHomeDesire = new ReturnHomeDesire();
            BeginGatheringDesire = new StartGatheringDesire();
            BeginIdleDesire = new IdleDesire();

            _priorityQueue.Enqueue(BeginGatheringDesire, 1.0f);
            _priorityQueue.Enqueue(ReturnHomeDesire, 1.0f);
            _priorityQueue.Enqueue(BeginIdleDesire, 1.0f);

            InvokeRepeating(nameof(UpdateStateChange), 0.1f, 0.1f);
        }

        protected virtual void InitVariables()
        {
        }

        public void ChangeState(State<Villager> s)
        {
            _fsm.ChangeState(s);
        }


        void UpdateStateChange()
        {
            Debug.Log("Update State Change Updates Every 0.1 Seconds");

            foreach (Desire desire in _priorityQueue)
            {
                desire.CalculateDesireValue(this);
                _priorityQueue.UpdatePriority(desire, desire.DesireVal);
            }

            State<Villager> potentialState = _priorityQueue.Last().State;

            if (!_fsm.CheckCurrentState(potentialState))
            {
                ChangeState(potentialState);
            }
        }


        private void Start()
        {

            #region SpawnTextBar

            AIPopUp = Instantiate(AIPopUp, transform);

            // TextMesh Pro Implementation
            textMeshPro = AIPopUp.transform.Find("Text Area").gameObject.AddComponent<TextMeshProUGUI>();

            textMeshPro.rectTransform.sizeDelta = new Vector2(3, 1);

            textMeshPro.color = Color.black;
            textMeshPro.enableAutoSizing = true;
            textMeshPro.fontSizeMin = 0;

            textMeshPro.alignment = TextAlignmentOptions.Center;

            textMeshPro.enableKerning = false;
            textMeshPro.text = string.Empty;
            //textMeshPro.isTextObjectScaleStatic = true;

            #endregion



            bb = GetComponent<VillagerBB>();


            #region Chopping Tree Behaviour Tree

            ChopTreeSequenceRoot = new Sequence(bb);

            //Chop Tree Selector
            CompositeNode chopTreeSelector = new Selector(bb);

            //Find and move to tree sequence Sequence
            CompositeNode findAndMoveToTreeSequence = new Sequence(bb);

            //Chop Sequence Node
            CompositeNode chopSequence = new Sequence(bb);

            //Chop Sequence Decorator Node
            ChopTreeDecorator chopDecorator = new ChopTreeDecorator(chopSequence, bb);

            //Find an available tree Decorator Node
            FindTreeDecorator findTreeDecorator = new FindTreeDecorator(findAndMoveToTreeSequence, bb);

            //Chop Tree Sequence

            ChopTreeSequenceRoot.AddChild(new GetMoveToLocation(bb,
                LocationNames.Forest)); // gets the location to move towards
            ChopTreeSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination
            ChopTreeSequenceRoot.AddChild(new VillagerWaitTillAtLocation(bb, this)); // wait till we reached destination
            ChopTreeSequenceRoot.AddChild(chopTreeSelector);

            //Chop Tree Selector

            chopTreeSelector.AddChild(findTreeDecorator);
            chopTreeSelector.AddChild(chopDecorator);


            //Find and move to tree sequence sequence

            findAndMoveToTreeSequence.AddChild(new PickNearestTree(bb, this)); // pick the nearest tree to chop
            findAndMoveToTreeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination
            findAndMoveToTreeSequence.AddChild(new VillagerWaitTillAtLocation(bb,
                this)); // wait till we reached destination

            //CHOP Sequence

            chopSequence.AddChild(new DelayNode(bb, GatheringSpeed, this)); // wait for 2 seconds
            chopSequence.AddChild(new ChopTree(bb, this)); //chop tree while tree health is more than 0


            //SLIGHT ISSUE WHERE IT'LL START ADDING TO THE WOOD before the player reaches the tree but thats fine.

            #endregion

            #region Go Home

            CompositeNode goHomeSequence = new Sequence(bb);

            CompositeNode restSequence = new Sequence(bb);

            GoHomeDecoratorRoot = new GoHomeDecorator(goHomeSequence, bb, this);

            goHomeSequence.AddChild(new SetMoveToHome(bb, this)); //Set Home Location
            goHomeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            goHomeSequence.AddChild(new VillagerWaitTillAtLocation(bb, this)); // wait till we reached destination
            goHomeSequence.AddChild(new EnterHome(bb, this)); // "enter the home"
            goHomeSequence.AddChild(restSequence);

            restSequence.AddChild(new ReplenishHealthAndStamina(bb, this));
            restSequence.AddChild(new DelayNode(bb, 2, this));

            #endregion

            #region Idle

            IdleSequenceRoot = new Sequence(bb);

            IdleSequenceRoot.AddChild(new PickRandomLocationNearby(bb, this)); //Set Home Location
            IdleSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            IdleSequenceRoot.AddChild(new VillagerWaitTillAtLocation(bb, this)); // wait till we reached destination
            IdleSequenceRoot.AddChild(new DelayNode(bb, 5, this));
            //TODO ASK J HOW TO LOOP BEHAVIOUR TREES

            #endregion

            ////Execute current BT every 0.1 seconds
            InvokeRepeating(nameof(UpdateFsm), 0.1f, 0.1f);
        }


        public void VillagerMoveTo(Vector3 moveLocation)
        {
            //if (!navMesh.SetDestination(moveLocation))
            //{
            //    Debug.Log(this + " failed to set destination, perhaps the location was inaccessible");
            //}
            ////TODO: IF THE AI CANNOT REACH THE DESTINATION IT CURRENTLY STOPS HIM FROM MOVING. DO SOMETHING WITH THIS SO IT DOESNT MESS EVERYTHING UP PLS
            //navMesh.isStopped = false;
        }

        public void StopMovement()
        {
            //navMesh.isStopped = true;
        }

        public void UpdateFsm()
        {
            _fsm.Update();
        }

        public void ExecuteBt()
        {
            BtRootNode.Execute();
        }

        void ChangeBehaviourTree(BtNode rootNode)
        {
            BtRootNode = rootNode;
        }

        public void StartChoppingTreesBt()
        {
            ChangeBehaviourTree(ChopTreeSequenceRoot);
        }

        public void StartGoHomeAndSleepBt()
        {
            ChangeBehaviourTree(GoHomeDecoratorRoot);
        }

        public void StartIdleBt()
        {
            ChangeBehaviourTree(IdleSequenceRoot);
        }


        //TODO: MAYBE FIX THE POPUP BUBBLE AND MAKE IT ACTUALLY WORK FOR ONCE

        public void UpdateAIText(object message)
        {
            textMeshPro.text = message.ToString();
        }


    }
}

