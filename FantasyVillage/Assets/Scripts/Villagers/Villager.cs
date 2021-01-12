using Assets.BehaviourTrees;
using Assets.Scripts.FiniteStateMachine;
using BehaviourTrees.VillagerBlackboards;
using Desires;
using LocationThings;
using Priority_Queue;
using States;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityTheory;
using static BehaviourTrees.CuttingTreeNodes;
using static BehaviourTrees.GenericNodes;
using static BehaviourTrees.HomeNodes;
using Random = UnityEngine.Random;

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

        public float IdleDesireValue;
        public float StartGatheringDesireValue;
        public float ReturnHomeDesireValue;


        public Desire DesireReturnHome;
        public Desire DesireBeginGathering;
        public Desire DesireBeginIdle;
        public Desire DesireBeginRepairingHouse;

        public bool bIsMoving = false;
        public Vector3 MoveToLocation;

        public float MinDistanceToMovePos;

        public GameObject Houses;

        public Villager(StateMachine<Villager> fsm)
        {
            this.fsm = fsm;
        }

        #region VillagerVariables


        [SerializeField] private float health;

        [SerializeField] private float maxHealth;

        [SerializeField] private float stamina;

        [SerializeField] private float maxStamina;

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

        [SerializeField] private float returnHomeBias;

        [SerializeField] private float startGatheringBias;

        [SerializeField] private float repairHouseBias;

        [SerializeField]
        private float idleBias;

        [SerializeField]
        protected float staminaLoss;



        /// 
        /// Getters and Setters
        /// 
        public float ReturnHomeBias
        {
            get => returnHomeBias;
            set => returnHomeBias = value;
        }

        public float RepairHouseBias
        {
            get => repairHouseBias;
            set => repairHouseBias = value;
        }
        public float StartGatheringBias
        {
            get => startGatheringBias;
            set => startGatheringBias = value;
        }

        public float Health
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

        public float Stamina
        {
            get => stamina;
            set => stamina = value;
        }

        public float IdleBias
        {
            get => idleBias;
            set => idleBias = value;
        }

        public float MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public float MaxStamina
        {
            get => maxStamina;
            set => maxStamina = value;
        }

        public HouseScript[] homes;

        //Behaviour Trees
        public BtNode BtRootNode;

        //Get reference to villagerRef Blackboard
        public VillagerBB bb;

        //Chop Trees Sequence Root
        public CompositeNode ChopTreeSequenceRoot;

        //Go Home Sequence Root
        public GoHomeDecorator GoHomeDecoratorRoot;

        //Repair House Sequence Root
        public CanRepairHomeDecorator RepairHouseRoot;

        //Idle Sequence Root
        public CompositeNode IdleSequenceRoot;


        #endregion

        public void Awake()
        {
            homes = Houses.GetComponentsInChildren<HouseScript>();

            var rndHome = Random.Range(0, homes.Length - 1);

            home = homes[rndHome].door;

            MaxHealth = 100;
            MaxStamina = 100;
            InitVariables();

            fsm = new StateMachine<Villager>();
            fsm.Configure(this, State_Default.Instance);

            priorityQueue = new SimplePriorityQueue<Desire>();

            DesireReturnHome = new Desire_ReturnHome();
            DesireBeginGathering = new Desire_StartGathering();
            DesireBeginIdle = new Desire_Idle();
            DesireBeginRepairingHouse = new Desire_RepairHouse();

            priorityQueue.Enqueue(DesireBeginGathering, 1.0f);
            priorityQueue.Enqueue(DesireReturnHome, 1.0f);
            priorityQueue.Enqueue(DesireBeginIdle, 1.0f);
            priorityQueue.Enqueue(DesireBeginRepairingHouse, 1.0f);

            InvokeRepeating(nameof(UpdateStateChange), 0.2f, 0.2f);
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
            Debug.Log("Update State Change Updates Every 0.2 Seconds");

            foreach (var desire in priorityQueue)
            {
                desire.CalculateDesireValue(this);
                priorityQueue.TryUpdatePriority(desire, 1 - desire.DesireVal);
            }

            var potentialState = priorityQueue.First.State;

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
            var chopDecorator = new ChopTreeDecorator(chopSequence, bb);

            //Find an available tree Decorator Node
            var findTreeDecorator = new FindTreeDecorator(findAndMoveToTreeSequence, bb);

            //Chop Tree Sequence

            ChopTreeSequenceRoot.AddChild(new GetMovePath(bb, LocationPositions.GetPositionFromLocation(LocationNames.Forest), this)); // gets the location to move towards
            ChopTreeSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination
            ChopTreeSequenceRoot.AddChild(chopTreeSelector);

            //Chop Tree Selector

            chopTreeSelector.AddChild(findTreeDecorator);
            chopTreeSelector.AddChild(chopDecorator);


            //Find and move to tree sequence sequence

            findAndMoveToTreeSequence.AddChild(new GetPathToNearestTree(bb, this)); // pick the nearest tree to chop
            findAndMoveToTreeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the calculated destination

            //the big CHOP Sequence

            chopSequence.AddChild(new DelayNode(bb, GatheringSpeed, this)); // wait for 2 seconds
            chopSequence.AddChild(new ChopTree(bb, this)); //chop tree while tree health is more than 0


            //TODO: SLIGHT ISSUE WHERE IT'LL START ADDING TO THE WOOD before the player reaches the tree but thats fine.

            #endregion

            #region Go Home

            CompositeNode goHomeSequence = new Sequence(bb);

            CompositeNode restSequence = new Sequence(bb);

            GoHomeDecoratorRoot = new GoHomeDecorator(goHomeSequence, bb, this);

            goHomeSequence.AddChild(new GetMovePath(bb, home.transform.position, this));
            goHomeSequence.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            goHomeSequence.AddChild(new CanRepairHomeDecorator.EnterHome(bb, this)); // "enter the home"
            goHomeSequence.AddChild(restSequence);

            restSequence.AddChild(new CanRepairHomeDecorator.ReplenishHealthAndStamina(bb, this));
            restSequence.AddChild(new DelayNode(bb, 2, this));

            #endregion

            #region Idle

            IdleSequenceRoot = new Sequence(bb);

            IdleSequenceRoot.AddChild(new GetPathToRandomNearbyLocation(bb, this)); //Set Home Location
            IdleSequenceRoot.AddChild(new VillagerMoveTo(bb, this)); // move to the destination
            IdleSequenceRoot.AddChild(new DelayNode(bb, 5, this));

            #endregion

            #region Repair House

            CompositeNode MoveToNearestBrokenHouse = new Sequence(bb);

            CompositeNode actuallyFixHouseSequence = new Sequence(bb);

            CompositeNode FixHouseSelector = new Selector(bb);

            var fixDecorator = new CanRepairHomeDecorator(actuallyFixHouseSequence, bb, this);



            #endregion

            ////Execute current BT every 0.1 seconds
            InvokeRepeating(nameof(UpdateFsm), 0.1f, 0.1f);
        }


        private void Update()
        {
            if (!bIsMoving) return;

            if (MoveToLocation.x <= float.MinValue || MoveToLocation.x >= float.MaxValue)
            {

                if (bb.AStarPath.Count <= 0) return;
                bb.AStarPath.Remove(bb.AStarPath.Last());
            }

            if (MoveToLocation.z <= float.MinValue || MoveToLocation.z >= float.MaxValue)
            {

                if (bb.AStarPath.Count <= 0) return;
                bb.AStarPath.Remove(bb.AStarPath.Last());
            }

            var position = transform.position;
            var direction = MoveToLocation - position;

            //Move the villager towards the next point
            position += direction.normalized * (Time.deltaTime * MoveSpeed);
            transform.position = position;


            transform.rotation = Quaternion.LookRotation(direction);

            //deplete stamina 
            stamina -= 5 * Time.deltaTime * staminaLoss;

            //checks if its close enough to the next point  
            if (Vector3.Distance(transform.position, MoveToLocation) < MinDistanceToMovePos)
            {
                bIsMoving = false;
                if (bb.AStarPath.Count <= 0) return;
                bb.AStarPath.Remove(bb.AStarPath.Last());
            }
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

        public void StartRepairHouse()
        {
            ChangeBehaviourTree(RepairHouseRoot);
        }
        public void StartIdleBt()
        {
            ChangeBehaviourTree(IdleSequenceRoot);
        }

        public void UpdateAIText(object message)
        {
            textMeshPro.text = message.ToString();
        }

        public void ChangeVisibility(bool isVisible)
        {
            foreach (var childRenderer in GetComponentsInChildren<Renderer>())
            {
                childRenderer.enabled = isVisible;
            }
        }

    }
}

