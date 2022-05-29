using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Core;

public class GameLoop
{
    private readonly ActorPool _actorPool;
    private readonly CollisionService _collisionService;

    public GameLoop(ActorPool actorPool, CollisionService collisionService)
    {
        _actorPool = actorPool;
        _collisionService = collisionService;
    }

    public void Update(float deltaTime)
    {
        UpdateSimulation();

        foreach (var actor in _actorPool.Actors)
        {
            actor.TakeTurn(deltaTime)?.Perform(deltaTime);
        }
    }

    private void UpdateSimulation()
    {
        foreach (var actor in _actorPool.Actors)
        {
            actor.ClearSimulationData();
        }
        for (int steps = 0; steps < 2; steps++)
        {
            _collisionService.Update();
        }
    }
}