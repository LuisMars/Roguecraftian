using Microsoft.Xna.Framework;
using Roguecraft.Engine.Core;

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

    public Matrix? GetViewTransformationMatrix()
    {
        return _camera.GetViewTransformationMatrix();
    }

    internal void Update(int width, int height)
    {
        _camera.SetPosition(_actorPool.Hero.Position);
        _camera.Update(width, height);
    }
}