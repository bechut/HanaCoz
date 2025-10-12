namespace HanaCoz.Utils.FiniteStateMachine;

public interface IState<in T>
{
    int Priority => 0;
    void Enter(T owner);
    void Update(T owner, double delta);
    void PhysicsUpdate(T owner, double delta);
    void Exit(T owner);
    
    IState<T> GetNextState(T owner); 
}
