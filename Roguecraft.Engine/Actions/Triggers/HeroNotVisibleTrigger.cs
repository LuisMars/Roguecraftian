namespace Roguecraft.Engine.Actions.Triggers;

public class HeroNotVisibleTrigger : ActionTrigger
{
    public override bool Trigger(ActionTriggerArgs args)
    {
        return !(args.Actor?.Visibility.IsVisibleByHero ?? false);
    }
}