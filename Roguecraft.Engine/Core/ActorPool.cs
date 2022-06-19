using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Core;

public class ActorPool
{
    private readonly List<Actor> _actors = new();
    private readonly List<Actor> _newActors = new();

    public IReadOnlyList<Actor> Actors => _actors;
    public Hero Hero { get; set; }

    public void Add(Actor actor)
    {
        _actors.Add(actor);
    }

    internal void AddLater(Item item)
    {
        _newActors.Add(item);
    }

    internal void AddNewActors()
    {
        _actors.AddRange(_newActors);
        _newActors.Clear();
    }

    internal void RemoveDead()
    {
        _actors.RemoveAll(a => a.IsPickedUp);
    }
}