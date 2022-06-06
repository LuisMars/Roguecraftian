using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions;

public abstract class GameAction
{
    public GameAction(Creature creature)
    {
        Creature = creature;
    }

    public Creature Creature { get; set; }
    public float EngeryCost { get; set; } = 0;

    public void Perform(float deltaTime)
    {
        OnPerform(deltaTime);

        Creature.Energy -= EngeryCost;
    }

    public virtual bool TryPrepare()
    {
        return true;
    }

    protected abstract void OnPerform(float deltaTime);
}