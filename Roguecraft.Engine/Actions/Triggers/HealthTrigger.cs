namespace Roguecraft.Engine.Actions.Triggers;

public class HealthTrigger : ActionTrigger
{
    private readonly int _health;

    public HealthTrigger(int health)
    {
        _health = health;
    }

    public override bool Trigger(ActionTriggerArgs args)
    {
        if (args.Actor is null)
        {
            return false;
        }
        return args.Actor.Health == _health;
    }
}