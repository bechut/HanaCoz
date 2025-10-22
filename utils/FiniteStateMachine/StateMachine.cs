using System.Collections.Generic;
using System.Linq;

namespace HanaCoz.Utils.FiniteStateMachine;

public class StateMachine<T>(T owner)
{
    private IState<T> _currentState;
    private readonly List<IState<T>> _registeredStates = [];

    // optional: register states for global checking
    public void RegisterState(IState<T> state) => _registeredStates.Add(state);

    public void ChangeState(IState<T> newState)
    {
        _currentState?.Exit(owner);
        _currentState = newState;
        _currentState?.Enter(owner);
    }

    public void Update(double delta)
    {
        _currentState?.Update(owner, delta);
    }

    public void PhysicsUpdate(double delta)
    {
        _currentState?.PhysicsUpdate(owner, delta);

        // Gather all possible transitions
        var candidates = new List<IState<T>>();

        // 1️⃣ ask the current state first
        var currentNext = _currentState?.GetNextState(owner);
        if (currentNext != null)
            candidates.Add(currentNext);

        // 2️⃣ check other states that may want to take over (interrupt)
        candidates.AddRange(
            _registeredStates
                .Where(state => state != _currentState)
                .Select(state => state.GetNextState(owner))
                .Where(next => next != null)!
        );

        // 3️⃣ pick the highest-priority candidate
        var nextState = candidates.OrderByDescending(s => s.Priority).FirstOrDefault();

        if (nextState != null && nextState != _currentState)
            ChangeState(nextState);
    }
}