namespace Roguecraft.Engine.Actions.Triggers;

public class DeathTrigger : ActionTrigger
{
    public DeathTrigger()
    {
    }

    public override bool Trigger(ActionTriggerArgs args)
    {
        if (args.Actor is null)
        {
            return false;
        }
        return args.Actor.Health <= 0;
    }
}