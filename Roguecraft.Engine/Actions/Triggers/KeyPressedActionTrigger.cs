using Microsoft.Xna.Framework.Input;

namespace Roguecraft.Engine.Actions.Triggers;

public class KeyPressedActionTrigger : ActionTrigger
{
    public List<Keys> Except { get; set; } = new();
    public List<Keys> Keys { get; set; } = new();

    public override bool Trigger(ActionTriggerArgs args)
    {
        if (args.State is null)
        {
            return false;
        }
        return Keys.All(k => args.State.Value.IsKeyDown(k)) && !Except.Any(k => args.State.Value.IsKeyDown(k));
    }
}