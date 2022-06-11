using Roguecraft.Engine.Actions.Triggers;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

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

    public GameAction GetNextAction(InputState state)
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
        var mouseAction = GetNextAction(args, true);
        if (mouseAction is not null && mouseAction is not NullAction)
        {
            return mouseAction;
        }
        var normalAction = GetNextAction(args, false);
        if (normalAction is not null)
        {
            return normalAction;
        }
        return _nullAction;
    }

    private GameAction GetNextAction(ActionTriggerArgs args, bool useMouse)
    {
        foreach (var (trigger, action) in Actions)
        {
            if (!trigger.Trigger(args) || action.IsIgnored(useMouse) || !action.TryPrepare(useMouse))
            {
                continue;
            }

            return action;
        }
        return null;
    }
}