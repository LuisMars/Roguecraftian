using Microsoft.Xna.Framework;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Cameras;

public class CameraService
{
    private readonly ActorPool _actorPool;
    private readonly Camera2D _camera;
    private readonly RandomGenerator _random;

    public CameraService(RandomGenerator randomGenerator, ActorPool actorPool, int width, int height)
    {
        _camera = new Camera2D(width, height);
        _actorPool = actorPool;
        _random = randomGenerator;
    }

    public Matrix GetViewTransformationMatrix()
    {
        return _camera.GetViewTransformationMatrix();
    }

    public Vector2 ScreenToWorld(Vector2 screen)
    {
        return _camera.ToWorld(screen);
    }

    internal void Update(int width, int height, float deltaTime)
    {
        var hero = _actorPool.Hero;
        var speed = hero.Stats.Speed;
        var currentSpeed = hero.Speed / deltaTime;
        var relativeSpeed = currentSpeed / speed;

        var cameraPosition = hero.Position;
        if (_camera.Position != Vector2.Zero)
        {
            cameraPosition = _camera.Position;
        }
        var target = hero.Position + currentSpeed * 2;
        var center = Vector2.Lerp(cameraPosition, target, deltaTime);
        _camera.SetPosition(center);
        _camera.Zoom = Math.Max(0.5f, MathHelper.Lerp(_camera.Zoom, 1 - relativeSpeed.Length(), deltaTime));
        _camera.Rotation = 0;
        _camera.Update(width, height);
        if (_actorPool.Hero.Timers[TimerType.Hurt]?.IsActive ?? false)
        {
            Shake();
        }
    }

    private void Shake()
    {
        _camera.Position += _random.RandomVector(64);
        _camera.Rotation += (_random.RandomNormal() - 0.5f) * 0.125f;
    }
}