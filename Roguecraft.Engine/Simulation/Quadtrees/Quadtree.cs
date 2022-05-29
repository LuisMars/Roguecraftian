using MonoGame.Extended;

namespace Roguecraft.Engine.Simulation.Quadtrees;

public class Quadtree
{
    public const int DefaultMaxDepth = 7;

    public const int DefaultMaxObjectsPerNode = 25;

    protected List<Quadtree> Children = new();

    protected HashSet<QuadtreeData> Contents = new();

    public Quadtree(RectangleF bounds)
    {
        CurrentDepth = 0;
        NodeBounds = bounds;
    }

    public bool IsLeaf => Children.Count == 0;

    public RectangleF NodeBounds { get; protected set; }

    protected int CurrentDepth { get; set; }

    protected int MaxDepth { get; set; } = 7;

    protected int MaxObjectsPerNode { get; set; } = 25;

    public void Insert(QuadtreeData data)
    {
        var bounds = data.Target.Bounds;
        if (!NodeBounds.Intersects(bounds))
        {
            return;
        }

        if (IsLeaf && Contents.Count >= MaxObjectsPerNode)
        {
            Split();
        }

        if (IsLeaf)
        {
            AddToLeaf(data);
            return;
        }

        foreach (var child in Children)
        {
            child.Insert(data);
        }
    }

    public int NumTargets()
    {
        var list = new List<QuadtreeData>();
        var num = 0;
        var queue = new Queue<Quadtree>();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            var quadtree = queue.Dequeue();
            if (!quadtree.IsLeaf)
            {
                foreach (Quadtree child in quadtree.Children)
                {
                    queue.Enqueue(child);
                }

                continue;
            }

            foreach (var content in quadtree.Contents.Where(c => !c.Dirty))
            {
                num++;
                content.MarkDirty();
                list.Add(content);
            }
        }

        for (var i = 0; i < list.Count; i++)
        {
            list[i].MarkClean();
        }

        return num;
    }

    public List<QuadtreeData> Query(IShapeF area)
    {
        var list = new List<QuadtreeData>();

        QueryWithoutReset(area, list);

        for (var i = 0; i < list.Count; i++)
        {
            list[i].MarkClean();
        }

        return list;
    }

    public void Remove(QuadtreeData data)
    {
        if (!IsLeaf)
        {
            throw new InvalidOperationException("Cannot remove from a non leaf Quadtree");
        }
        data.RemoveParent(this);
        Contents.Remove(data);
    }

    public void Shake()
    {
        if (IsLeaf)
        {
            return;
        }

        var list = new List<QuadtreeData>();
        var num = NumTargets();
        if (num == 0)
        {
            Children.Clear();
        }
        else if (num < MaxObjectsPerNode)
        {
            var queue = new Queue<Quadtree>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var quadtree = queue.Dequeue();
                if (!quadtree.IsLeaf)
                {
                    foreach (var child in quadtree.Children)
                    {
                        queue.Enqueue(child);
                    }

                    continue;
                }

                foreach (var content in quadtree.Contents.Where(c => !c.Dirty))
                {
                    AddToLeaf(content);
                    content.MarkDirty();
                    list.Add(content);
                }
            }

            Children.Clear();
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i].MarkClean();
        }
    }

    public void Split()
    {
        if (CurrentDepth + 1 >= MaxDepth)
        {
            return;
        }

        var topLeft = NodeBounds.TopLeft;
        var bottomRight = NodeBounds.BottomRight;
        var center = NodeBounds.Center;
        var array = new RectangleF[4]
        {
                RectangleF.CreateFrom(topLeft, center),
                RectangleF.CreateFrom(new Point2(center.X, topLeft.Y), new Point2(bottomRight.X, center.Y)),
                RectangleF.CreateFrom(center, bottomRight),
                RectangleF.CreateFrom(new Point2(topLeft.X, center.Y), new Point2(center.X, bottomRight.Y))
        };
        for (var i = 0; i < array.Length; i++)
        {
            var item = new Quadtree(array[i]);
            Children.Add(item);
            Children[i].CurrentDepth = CurrentDepth + 1;
        }

        foreach (var content in Contents)
        {
            foreach (var child in Children)
            {
                child.Insert(content);
            }
        }

        Clear();
    }

    private void AddToLeaf(QuadtreeData data)
    {
        data.AddParent(this);
        Contents.Add(data);
    }

    private void Clear()
    {
        foreach (var content in Contents)
        {
            content.RemoveParent(this);
        }

        Contents.Clear();
    }

    private void QueryWithoutReset(IShapeF area, List<QuadtreeData> recursiveResult)
    {
        if (!NodeBounds.Intersects(area))
        {
            return;
        }

        if (IsLeaf)
        {
            foreach (var content in Contents.Where(content => !content.Dirty && content.Bounds.Intersects(area)))
            {
                recursiveResult.Add(content);
                content.MarkDirty();
            }
            return;
        }

        var i = 0;
        for (var count = Children.Count; i < count; i++)
        {
            Children[i].QueryWithoutReset(area, recursiveResult);
        }
    }
}