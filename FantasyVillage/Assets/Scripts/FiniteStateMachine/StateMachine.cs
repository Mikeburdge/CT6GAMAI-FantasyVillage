namespace Assets.Scripts.FiniteStateMachine
{
    public class StateMachine<T>
    {
        private T _owner;
        private State<T> _currentState;
        private State<T> _previousState;
        private State<T> _globalState;

        public void Awake()
        {
            _currentState = null;
            _previousState = null;
            _globalState = null;
        }

        public void Configure(T owner, State<T> initialState)
        {
            _owner = owner;
            ChangeState(initialState);
        }

        public void Update()
        {
            if (_globalState != null) _globalState.Execute(_owner);
            if (_currentState != null) _currentState.Execute(_owner);
        }




        /// <summary>
        ///Returns true if the current state is equal to the given state
        /// </summary>
        public bool CheckCurrentState(State<T> inState)
        {
            if (inState == _currentState)
            {
                return true;
            }

            return false;
        }

        public void ChangeState(State<T> newState)
        {
            _previousState = _currentState;
            _currentState?.Exit(_owner);
            _currentState = newState;
            _currentState?.Enter(_owner);
        }

        public void RevertToPreviousState()
        {
            if (_previousState != null)
                ChangeState(_previousState);

        }
    }
}