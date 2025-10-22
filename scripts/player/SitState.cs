using HanaCoz.Utils.FiniteStateMachine;

namespace HanaCoz.Scripts.Player;

public class SitState : IState<Player>
{
    public int Priority => 0;
    private static string Anim => "sit";

    public void Enter(Player owner)
    {
        owner.IsSit = true;
    }
    public void Exit(Player owner) 
    {
        owner.IsSit = false;
    }
    public void Update(Player owner, double delta) { }

    public void PhysicsUpdate(Player owner, double delta)
    {
        var dir = owner.LastDir == "l" ? "l" : "r";
        owner.Anim.Play($"{Anim}{dir}");
        owner.Outfit.Play($"{Anim}{dir}");
    }

    public IState<Player> GetNextState(Player owner)
    {
        return owner.Velocity.LengthSquared() > 0.1 ? owner.States[Player.PlayerStateType.Run] : null;
    }
}
