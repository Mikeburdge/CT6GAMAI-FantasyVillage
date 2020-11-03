namespace Assets.Scripts.FiniteStateMachine
{
    public class StateMachine<T>
    {
        private T Owner;
        private State<T> CurrentState;
        private State<T> PreviousState;
        private State<T> GlobalState;

        public void Awake()
        {
            CurrentState = null;
            PreviousState = null;
            GlobalState = null;
        }

        public void Configure(T owner, State<T> InitialState)
        {
            Owner = owner;
            ChangeState(InitialState);
        }

        public void Update()
        {
            if (GlobalState != null) GlobalState.Execute(Owner);
            if (CurrentState != null) CurrentState.Execute(Owner);
        }




        /// <summary>
        ///Returns true if the current state is equal to the given state
        /// </summary>
        public bool CheckCurrentState(State<T> inState)
        {
            if (inState == CurrentState)
            {
                return true;
            }

            return false;
        }

        public void ChangeState(State<T> NewState)
        {
            PreviousState = CurrentState;
            if (CurrentState != null)
                CurrentState.Exit(Owner);
            CurrentState = NewState;
            if (CurrentState != null)
                CurrentState.TravelTo(Owner);
        }

        public void RevertToPreviousState()
        {
            if (PreviousState != null)
                ChangeState(PreviousState);

        }
    }
}