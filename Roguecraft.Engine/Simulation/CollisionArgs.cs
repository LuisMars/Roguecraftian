using Microsoft.Xna.Framework;
using Roguecraft.Engine.Components;

namespace Roguecraft.Engine.Simulation;

public class CollisionArgs
{
    public Collision Other { get; internal set; }

    public Vector2 PenetrationVector { get; internal set; }
}