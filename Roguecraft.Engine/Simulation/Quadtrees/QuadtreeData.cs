using MonoGame.Extended;
using Roguecraft.Engine.Components;

namespace Roguecraft.Engine.Simulation.Quadtrees;

public class QuadtreeData
{
    public HashSet<Quadtree> Parents = new();

    public QuadtreeData(Collision target)
    {
        Target = target;
        Bounds = target.Bounds;
    }

    public IShapeF Bounds { get; set; }

    public bool Dirty { get; private set; }

    public Collision Target { get; set; }

    public void AddParent(Quadtree parent)
    {
        Parents.Add(parent);
    }

    public void MarkClean()
    {
        Dirty = false;
    }

    public void MarkDirty()
    {
        Dirty = true;
    }

    public void RemoveFromAllParents()
    {
        foreach (var item in Parents)
        {
            item.Remove(this);
        }

        Parents.Clear();
    }

    public void RemoveParent(Quadtree parent)
    {
        Parents.Remove(parent);
    }
}