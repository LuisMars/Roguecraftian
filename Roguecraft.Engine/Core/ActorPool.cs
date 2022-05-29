using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Core;

public class ActorPool
{
    private readonly List<Actor> _actors = new();

    public IReadOnlyList<Actor> Actors => _actors;
    public Hero Hero { get; set; }

    public void Add(Actor actor)
    {
        _actors.Add(actor);
    }
}