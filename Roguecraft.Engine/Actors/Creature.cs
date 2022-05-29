using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Components;

namespace Roguecraft.Engine.Actors;

public abstract class Creature : Actor
{
    public Collision AreaOfInfluence { get; set; }
    public float Energy { get; set; }
    public int Health { get; set; }
    public Stats Stats { get; set; }
    public WalkAction WalkAction { get; set; }

    public override void ClearSimulationData()
    {
        base.ClearSimulationData();
        AreaOfInfluence.Clear();
    }

    public override void UpdateSimulationData()
    {
        base.UpdateSimulationData();
        AreaOfInfluence.Update();
    }
}