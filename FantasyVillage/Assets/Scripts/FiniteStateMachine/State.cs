using System.Collections;

namespace Assets.Scripts.FiniteStateMachine
{
    abstract public class State<T>
    {
        abstract public void TravelTo(T entity);
        abstract public void Enter(T entity);
        abstract public void Execute(T entity);
        abstract public void Exit(T entity);
    }
}