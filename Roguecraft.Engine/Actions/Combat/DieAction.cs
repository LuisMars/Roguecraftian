using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Actions.Combat;

public class DieAction : GameAction
{
    private readonly ActorPool _actorPool;
    private readonly CollisionService _collisionService;
    private readonly RandomGenerator _randomGenerator;

    public DieAction(Creature creature, ActorPool actorPool, CollisionService collisionService, RandomGenerator randomGenerator) : base(creature)
    {
        _actorPool = actorPool;
        _collisionService = collisionService;
        _randomGenerator = randomGenerator;
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Die();
        foreach (var item in Creature.Inventory.Items.Where(t => t is not null))
        {
            item.Drop(Creature, _randomGenerator, _collisionService, _actorPool);
        }
    }
}