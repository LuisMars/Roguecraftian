using Microsoft.Xna.Framework;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Cameras;

public class CameraService
{
    private readonly ActorPool _actorPool;
    private readonly Camera2D _camera;

    public CameraService(ActorPool actorPool, int width, int height)
    {
        _camera = new Camera2D(width, height);
        _actorPool = actorPool;
    }

    public Matrix GetViewTransformationMatrix()
    {
        return _camera.GetViewTransformationMatrix();
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
        _camera.Position += MathUtils.RandomVector(64);
        _camera.Rotation += (MathUtils.RandomNormal() - 0.5f) * 0.125f;
    }
}