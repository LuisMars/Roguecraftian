using MonoGame.Extended.Input;
using Roguecraft.Engine.Actions.Triggers;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions;

public class AvailableActions
{
    private readonly NullAction _nullAction;

    public AvailableActions(Creature creature)
    {
        _nullAction = new NullAction(creature);
    }

    private List<(ActionTrigger Trigger, GameAction Action)> Actions { get; set; } = new();

    public void Add(ActionTrigger trigger, GameAction action)
    {
        Actions.Add((trigger, action));
    }

    public GameAction GetNextAction(KeyboardStateExtended state)
    {
        var args = new ActionTriggerArgs
        {
            State = state
        };
        return GetNextAction(args);
    }

    public GameAction GetNextAction(Creature creature)
    {
        var args = new ActionTriggerArgs
        {
            Actor = creature
        };
        return GetNextAction(args);
    }

    private GameAction GetNextAction(ActionTriggerArgs args)
    {
        foreach (var (trigger, action) in Actions)
        {
            if (!trigger.Trigger(args) || !action.TryPrepare())
            {
                continue;
            }

            return action;
        }
        return _nullAction;
    }
}