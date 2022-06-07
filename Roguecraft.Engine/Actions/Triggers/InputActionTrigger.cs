using Microsoft.Xna.Framework;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Triggers;

public class InputActionTrigger : ActionTrigger
{
    public List<InputAction> Except { get; set; } = new();
    public List<InputAction> Keys { get; set; } = new();
    public bool LeftStick { get; set; } = false;

    public override bool Trigger(ActionTriggerArgs args)
    {
        if (args.State is null)
        {
            return false;
        }
        if (LeftStick && args.State.LeftJostick == Vector2.Zero)
        {
            return false;
        }
        return Keys.All(k => args.State.IsButtonDown(k)) && !Except.Any(k => args.State.IsButtonDown(k));
    }
}