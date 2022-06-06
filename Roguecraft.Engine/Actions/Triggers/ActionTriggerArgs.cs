using MonoGame.Extended.Input;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Triggers;

public class ActionTriggerArgs
{
    public Creature? Actor { get; init; }
    public KeyboardStateExtended? State { get; init; }
}