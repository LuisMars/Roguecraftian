using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Input;
using Roguecraft.Engine.Render;
using Roguecraft.Engine.Sound;

namespace Roguecraft.Engine.Core;

public class UpdateService
{
    private readonly ActorPool _actorPool;
    private readonly GameLoop _gameLoop;
    private readonly InputManager _inputManager;
    private readonly ParticleRenderer _particleRenderer;
    private readonly SoundService _soundService;
    private readonly TimeManager _timeManager;

    public UpdateService(InputManager inputManager,
                         TimeManager timeManager,
                         GameLoop gameLoop,
                         ActorPool actorPool,
                         SoundService soundService,
                         ParticleRenderer particleRenderer)
    {
        _inputManager = inputManager;
        _timeManager = timeManager;
        _gameLoop = gameLoop;
        _actorPool = actorPool;
        _soundService = soundService;
        _particleRenderer = particleRenderer;
    }

    public void Update(float deltaTime)
    {
        _inputManager.Update();
        deltaTime = _timeManager.GetDeltaTime(deltaTime);
        _gameLoop.Update(deltaTime);
        var hero = _actorPool.Hero;

        if (hero.Direction == Vector2.Zero && _inputManager.State.IsMouseUsed)
        {
            hero.Angle = (_inputManager.State.MousePosition - hero.Position).ToAngle();
        }
        _soundService.Play();
        _particleRenderer.Update(deltaTime);
        _gameLoop.UpdateTimers(deltaTime);
    }
}