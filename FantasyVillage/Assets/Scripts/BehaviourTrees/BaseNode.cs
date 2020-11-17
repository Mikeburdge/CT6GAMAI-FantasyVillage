using System.Collections.Generic;
using System.Timers;
using Assets.Scripts.Villagers;
using Villagers;

namespace Assets.BehaviourTrees
{

    /// <summary>
    /// Execute can return one of three things
    /// </summary>
    public enum BtStatus
    {
        Running,
        Success,
        Failure
    }

    /// <summary>
    /// Base class. Sets the foundations for everything else
    /// </summary>
    public abstract class BtNode
    {
        protected BaseBlackboard Bb;
        public BtNode(BaseBlackboard bb)
        {
            this.Bb = bb;
        }

        public abstract BtStatus Execute();

        /// <summary>
        /// Reset should be overidden in child classes as and when necessary
        /// It should be called when a node is abruptly aborted before it can finish with a success or failure
        /// i.e the node was still RUNNING when it is aborted you need to gracefully handle it to avoid unintended bugs
        /// See DelayNode, CompositeNode and DecoratorNode for examples
        /// </summary>
        public virtual void Reset()
        {

        }
    }

    /// <summary>
    /// Base class for node that can take child nodes. Only meant to be used in subclasses like Selector and Sequence,
    /// but you can add other subclass types (e.g. RandomSelector, RandomSequence, Parallel etc.)
    /// </summary>
    public abstract class CompositeNode : BtNode
    {
        protected int CurrentChildIndex;
        protected List<BtNode> Children;
        public CompositeNode(BaseBlackboard bb) : base(bb)
        {
            Children = new List<BtNode>();
        }
        public void AddChild(BtNode child)
        {
            Children.Add(child);
        }

        /// <summary>
        /// When a composite node is reset it set the child index back to 0, and it should propogate the reset down to all its children
        /// </summary>
        public override void Reset()
        {
            CurrentChildIndex = 0;
            //Reset every child
            for (int j = 0; j < Children.Count; j++)
            {
                Children[j].Reset();
            }
        }
    }

    /// <summary>
    /// Selectors execute their children in order until a child succeeds, at which point it stops execution
    /// If a child returns RUNNING, then it will need to stop execution but resume from the same point the next time it executes
    /// </summary>
    public class Selector : CompositeNode
    {
        public Selector(BaseBlackboard bb) : base(bb)
        {

        }

        public override BtStatus Execute()
        {
            BtStatus rv = BtStatus.Failure;


            for (int j = CurrentChildIndex; j < Children.Count; j++)
            {
                BtStatus currentChildStatus = Children[j].Execute();

                switch (currentChildStatus)
                {
                    case BtStatus.Running:
                        CurrentChildIndex = j;
                        return currentChildStatus;
                    case BtStatus.Success:
                        return currentChildStatus;
                    case BtStatus.Failure:
                        break;
                }
            }

            Reset();
            return rv;
        }
    }

    /// <summary>
    /// Sequences execute their children in order until a child fails, at which point it stops execution
    /// If a child returns RUNNING, then it will need to stop execution but resume from the same point the next time it executes
    /// </summary>
    public class Sequence : CompositeNode
    {
        public Sequence(BaseBlackboard bb) : base(bb)
        {
        }
        public override BtStatus Execute()
        {
            BtStatus rv = BtStatus.Failure;

            for (int j = CurrentChildIndex; j < Children.Count; j++)
            {
                BtStatus currentChildStatus = Children[j].Execute();

                switch (currentChildStatus)
                {
                    case BtStatus.Running:
                        CurrentChildIndex = j;
                        return currentChildStatus;
                    case BtStatus.Success:
                        break;
                    case BtStatus.Failure:
                        return currentChildStatus;
                }
            }
            Reset();
            return rv;
        }
    }

    /// <summary>
    /// Decorator nodes customise functionality of other nodes by wrapping around them, see InverterDecorator for example
    /// </summary>
    public abstract class DecoratorNode : BtNode
    {
        protected BtNode WrappedNode;
        public DecoratorNode(BtNode wrappedNode, BaseBlackboard bb) : base(bb)
        {
            this.WrappedNode = wrappedNode;
        }

        public BtNode GetWrappedNode()
        {
            return WrappedNode;
        }

        /// <summary>
        /// Should reset the wrapped node
        /// </summary>
        public override void Reset()
        {
            WrappedNode.Reset();
        }
    }


    /// <summary>
    /// Inverter decorator simply inverts the result of success/failure of the wrapped node
    /// </summary>
    public class InverterDecorator : DecoratorNode
    {
        public InverterDecorator(BtNode wrappedNode, BaseBlackboard bb) : base(wrappedNode, bb)
        {
        }

        public override BtStatus Execute()
        {
            BtStatus rv = WrappedNode.Execute();

            if (rv == BtStatus.Failure)
            {
                rv = BtStatus.Success;
            }
            else if (rv == BtStatus.Success)
            {
                rv = BtStatus.Failure;
            }

            return rv;
        }
    }

    /// <summary>
    /// Inherit this and override CheckStatus. If that returns true, then it will execute the WrappedNode otherwise it will return failure
    /// </summary>
    public abstract class ConditionalDecorator : DecoratorNode
    {
        public ConditionalDecorator(BtNode wrappedNode, BaseBlackboard bb) : base(wrappedNode, bb)
        {
        }

        public abstract bool CheckStatus();
        public override BtStatus Execute()
        {
            BtStatus rv = BtStatus.Failure;

            if (CheckStatus())
                rv = WrappedNode.Execute();

            return rv;
        }


    }

    /// <summary>
    /// This node simply returns success after the allotted delay time has passed
    /// </summary>
    public class DelayNode : BtNode
    {
        protected float Delay;
        bool _started;
        private Timer _regulator;
        bool _delayFinished;
        private Villager villagerRef;

        public DelayNode(BaseBlackboard bb, float delayTime, Villager villager) : base(bb)
        {
            villagerRef = villager;
            Delay = delayTime;
            _regulator = new Timer(Delay * 1000.0f); // in milliseconds, so multiply by 1000
            _regulator.Elapsed += OnTimedEvent;
            _regulator.Enabled = true;
            _regulator.Stop();
        }

        public override BtStatus Execute()
        {
            BtStatus rv = BtStatus.Running;
            villagerRef.UpdateAIText("Delay for " + Delay + " Seconds");

            if (!_started
                && !_delayFinished)
            {
                _started = true;
                _regulator.Start();
            }
            else if (_delayFinished)
            {
                _delayFinished = false;
                _started = false;
                rv = BtStatus.Success;
            }

            return rv;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _started = false;
            _delayFinished = true;
            _regulator.Stop();
        }

        //Timers count down independently of the Behaviour Tree, so we need to stop them when the behaviour is aborted/reset
        public override void Reset()
        {
            _regulator.Stop();
            _delayFinished = false;
            _started = false;
        }
    }

}