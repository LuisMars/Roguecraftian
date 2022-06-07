using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Triggers;

public class ActionTriggerArgs
{
    public Creature? Actor { get; init; }
    public InputState? State { get; init; }
}