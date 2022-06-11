using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Input;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Actions;

public abstract class GameAction
{
    public GameAction(Creature creature)
    {
        Creature = creature;
    }

    public GameAction(Creature creature, InputManager inputManager) : this(creature)
    {
        InputManager = inputManager;
    }

    public Creature Creature { get; set; }
    public bool IgnoreOnMouse { get; set; } = false;
    public InputManager InputManager { get; set; }
    protected float EngeryCost { get; set; } = 0;

    public CollisionArgs? GetSelected<TActor>(bool useMouse, Func<CollisionArgs, bool>? extraConditions = null) where TActor : Actor
    {
        if (useMouse)
        {
            if (InputManager is not null && InputManager.State.IsMouseEvent)
            {
                //var matches = Creature.AreaOfInfluence.Where<TActor>(x => x.Other.Actor.Collision.Bounds.Contains(InputManager.State.MousePosition));
                //if (matches.Any())
                //{
                //    return matches.FirstOrDefault(x => extraConditions?.Invoke(x) ?? true);
                //}
                return Creature.AreaOfInfluence.FirstOrDefault<TActor>(x => x.Other.Actor.Collision.Bounds.Contains(InputManager.State.MousePosition) &&
                                                                         (extraConditions?.Invoke(x) ?? true));
            }
            return null;
        }

        return Creature.AreaOfInfluence.Closest<TActor>(x => extraConditions?.Invoke(x) ?? true);
    }

    public bool IsIgnored(bool useMouse)
    {
        return useMouse && IgnoreOnMouse;
    }

    public void Perform(float deltaTime = 0)
    {
        OnPerform(deltaTime);

        Creature.Energy -= EngeryCost;
    }

    public virtual bool TryPrepare(bool useMouse)
    {
        return true;
    }

    protected abstract void OnPerform(float deltaTime);
}