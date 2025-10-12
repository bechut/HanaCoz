using Godot;
using HanaCoz.Utils.FiniteStateMachine;

namespace HanaCoz.Scripts.Player;

public class RunState : IState<Player>
{
    public int Priority => 0;
    private static string Anim => "run";
    public void Enter(Player owner) {}
    public void Exit(Player owner) {}
    public void Update(Player owner, double delta) { }

    public void PhysicsUpdate(Player owner, double delta)
    {
        foreach (var sprite in new[] { owner.Anim, owner.Outfit, owner.Hair })
            sprite.Play($"{Anim}{owner.LastDir}");
    }

    public IState<Player> GetNextState(Player owner)
    {
        return owner.Velocity.LengthSquared() < 0.1 ? owner.States[Player.PlayerStateType.Idle] : null;
    }
}
