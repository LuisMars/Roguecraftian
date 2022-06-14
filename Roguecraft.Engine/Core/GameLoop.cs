﻿using Microsoft.Xna.Framework.Input;
using Roguecraft.Engine.Simulation;
using Roguecraft.Engine.Visibility;

namespace Roguecraft.Engine.Core;

public class GameLoop
{
    private readonly ActorPool _actorPool;
    private readonly CollisionService _collisionService;
    private readonly VisibilityService _visibilityService;

    public GameLoop(ActorPool actorPool, CollisionService collisionService, VisibilityService visibilityService)
    {
        _actorPool = actorPool;
        _collisionService = collisionService;
        _visibilityService = visibilityService;
    }

    public void Update(float deltaTime)
    {
        _actorPool.RemoveDead();
        _collisionService.RemoveDead();
        _visibilityService.Update(_actorPool.Hero);

        foreach (var actor in _actorPool.Actors)
        {
            actor.CalculateVisibility(_visibilityService);

            actor.TakeTurn(deltaTime)?.Perform(deltaTime);
        }
        UpdateSimulation(deltaTime);
    }

    internal void UpdateTimers(float deltaTime)
    {
        foreach (var actor in _actorPool.Actors)
        {
            actor.AfterUpdate(deltaTime);
        }
    }

    private void UpdateSimulation(float deltaTime)
    {
        foreach (var actor in _actorPool.Actors)
        {
            actor.ClearSimulationData();
        }
        for (int steps = 0; steps < 1; steps++)
        {
            _collisionService.Update();
        }

        var strength = 1f * _actorPool.Hero.Timers.JustTriggeredTypes.Count();
        GamePad.SetVibration(0, strength, strength);
    }
}