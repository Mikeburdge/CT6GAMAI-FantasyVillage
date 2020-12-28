using System.Collections.Generic;
using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.FiniteStateMachine;
using Desires;
using LocationThings;
using Priority_Queue;
using States;
using System.Linq;
using PathfindingSection;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UtilityTheory;
using static BehaviourTrees.CuttingTreeNodes;
using static BehaviourTrees.GenericNodes;
using static BehaviourTrees.HomeNodes;

namespace Villagers
{
    [RequireComponent(typeof(VillagerBB))]
    public class Villager : Humanoid
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

        private StateMachine<Villager> fsm;

        private SimplePriorityQueue<Desire> priorityQueue = new SimplePriorityQueue<Desire>();

        public Desire ReturnHomeDesire;
        public Desire BeginGatheringDesire;
        public Desire BeginIdleDesire;

        public Villager(StateMachine<Villager> fSm)
        {
            fsm = fSm;
        }

        #region Variables


        [SerializeField] private int health;

        [SerializeField] private int maxHealth;

        [SerializeField] private int stamina;

        [SerializeField] private int maxStamina;

        [SerializeField] private GameObject home;

        [FormerlySerializedAs("Damage")]
        [SerializeField]
        protected int damage;

        [FormerlySerializedAs("AttackCooldown")]
        [SerializeField]
        protected float attackCooldown;

        //Combat Skill: Determines the this villagers Capabilities in Combat
        [FormerlySerializedAs("CombatSpeed")]
        [SerializeField]
        protected int combatSpeed;

        //Construction Skill: Determines the speed in which this villager can construct or repair buildings
        [FormerlySerializedAs("ConstructionSpeed")]
        [SerializeField]
        protected int constructionSpeed;

        //Farming Skill: Determines speed in which this villager can farm food
        [FormerlySerializedAs("FarmingSpeed")]
        [SerializeField]
        protected int farmingSpeed;

        //Gathering Skill: Determines the speed in which this villager gathers wood and rocks from trees and bigger rocks
        [SerializeField] private int gatheringSpeed;

        //public NavMeshAgent navMesh;

        [SerializeField] private float returnHomeBias;

        [SerializeField] private float startGatheringBias;

        [SerializeField]
        private float idleBias = Random.Range(0.1f, 2); //makes some villagers lazier than others



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

        //Get reference to villagerRef Blackboard
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

            fsm = new StateMachine<Villager>();
            fsm.Configure(this, State_Default.Instance);

            priorityQueue = new SimplePriorityQueue<Desire>();

            ReturnHomeDesire = new Desire_ReturnHome();
            BeginGatheringDesire = new Desire_StartGathering();
            BeginIdleDesire = new Desire_Idle();

            priorityQueue.Enqueue(BeginGatheringDesire, 1.0f);
            priorityQueue.Enqueue(ReturnHomeDesire, 1.0f);
            priorityQueue.Enqueue(BeginIdleDesire, 1.0f);

            InvokeRepeating(nameof(UpdateStateChange), 0.1f, 0.1f);
        }

        protected virtual void InitVariables()
        {
        }

        public void ChangeState(State<Villager> s)
        {
            fsm.ChangeState(s);
        }


        void UpdateStateChange()
        {
            Debug.Log("Update State Change Updates Every 0.1 Seconds");

            foreach (Desire desire in priorityQueue)
            {
                desire.CalculateDesireValue(this);
                priorityQueue.UpdatePriority(desire, desire.DesireVal);
            }

            State<Villager> potentialState = priorityQueue.Last().State;

            if (!fsm.CheckCurrentState(potentialState))
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

            ChopTreeSequenceRoot.AddChild(new GetMovePath(bb, LocationPositions.GetPositionFromLocation(LocationNames.Forest), this)); // gets the location to move towards
            //ChopTreeSequenceRoot.AddChild(new CheckAStarPath(bb, this)); // Checks the current AStarPath to see if its valid
            ChopTreeSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination
            ChopTreeSequenceRoot.AddChild(chopTreeSelector);

            //Chop Tree Selector

            chopTreeSelector.AddChild(findTreeDecorator);
            chopTreeSelector.AddChild(chopDecorator);


            //Find and move to tree sequence sequence

            findAndMoveToTreeSequence.AddChild(new GetPathToNearestTree(bb, this)); // pick the nearest tree to chop
            //findAndMoveToTreeSequence.AddChild(new CheckAStarPath(bb, this)); // Checks the current AStarPath to see if its valid
            findAndMoveToTreeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination

            //CHOP Sequence

            chopSequence.AddChild(new DelayNode(bb, GatheringSpeed, this)); // wait for 2 seconds
            chopSequence.AddChild(new ChopTree(bb, this)); //chop tree while tree health is more than 0


            //TODO: SLIGHT ISSUE WHERE IT'LL START ADDING TO THE WOOD before the player reaches the tree but thats fine.

            #endregion

            #region Go Home

            CompositeNode goHomeSequence = new Sequence(bb);

            CompositeNode restSequence = new Sequence(bb);

            GoHomeDecoratorRoot = new GoHomeDecorator(goHomeSequence, bb, this);

            goHomeSequence.AddChild(new GetMovePath(bb, LocationPositions.GetPositionFromLocation(LocationNames.Home), this));
            //goHomeSequence.AddChild(new CheckAStarPath(bb, this)); // move to the destination
            goHomeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            goHomeSequence.AddChild(new EnterHome(bb, this)); // "enter the home"
            goHomeSequence.AddChild(restSequence);

            restSequence.AddChild(new ReplenishHealthAndStamina(bb, this));
            restSequence.AddChild(new DelayNode(bb, 2, this));

            #endregion

            #region Idle

            IdleSequenceRoot = new Sequence(bb);

            IdleSequenceRoot.AddChild(new GetPathToRandomNearbyLocation(bb, this)); //Set Home Location
            //IdleSequenceRoot.AddChild(new CheckAStarPath(bb, this)); // Checks the current AStarPath to see if its valid
            IdleSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            IdleSequenceRoot.AddChild(new DelayNode(bb, 5, this));

            #endregion

            ////Execute current BT every 0.1 seconds
            InvokeRepeating(nameof(UpdateFsm), 0.1f, 0.1f);
        }


        public bool VillagerMoveAlongPath()
        {



            //sets the next point to that of the nearest node in the path (in perfect world nodes like this would work properly, but this is not a perfect world)
            var nextPoint = bb.AStarPath.Last();

            var removePoint = nextPoint;

            //sets the next points Y to be that of the villager as currently the nodes are on the ground and not adjusted properly for terrain (possible TODO)
            nextPoint.y = transform.position.y;

            //checks if its close enough to the next point
            if (Vector3.Distance(transform.position, nextPoint) < 1)
            {
                bb.AStarPath.Remove(removePoint);
            }

            //Move the villager towards the next point
            transform.position += (nextPoint - transform.position).normalized * Time.deltaTime * 20;

            //check if its reached the final node in the path, return true if it has and false if not
            return !(bb.AStarPath.Count > 0);
        }

        public void UpdateFsm()
        {
            fsm.Update();
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


        public void UpdateAIText(object message)
        {
            textMeshPro.text = message.ToString();
        }


    }
}

