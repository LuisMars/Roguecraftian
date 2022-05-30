using Microsoft.Xna.Framework;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

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
        var center = hero.Position;
        var speed = hero.Stats.Speed;
        var direction = hero.WalkAction.Direction;
        if (_camera.Position != Vector2.Zero)
        {
            center = Vector2.Lerp(_camera.Position, center + (direction * speed), deltaTime);
        }
        _camera.SetPosition(center);

        var newZoom = MathF.Max(0.33f, 1f - direction.Length());
        _camera.Zoom = MathHelper.Lerp(_camera.Zoom, newZoom, deltaTime);
        _camera.Rotation = 0;
        _camera.Update(width, height);
        if (_actorPool.Hero.HurtTimer?.IsActive ?? false)
        {
            Shake();
        }
    }

    private void Shake()
    {
        _camera.Position += MathUtils.RandomVector(128);
        _camera.Rotation += (MathUtils.RandomNormal() - 0.5f) * 0.0625f;
    }
}