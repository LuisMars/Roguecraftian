using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Visibility;

public interface IVisibilityComputer
{
    Vector2 Origin { get; }
    float Radius { get; }

    void AddLineOccluder(Vector2 p1, Vector2 p2, Actor actor);

    void ClearOccluders();

    List<Vector2> Compute(out List<Actor> actors);
}