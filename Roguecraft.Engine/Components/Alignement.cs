namespace Roguecraft.Engine.Components;

public class Alignement
{
    public int Friendly { get; set; }
    public int Group { get; set; }
    public int Hostile { get; set; }

    public bool IsHostileTo(Alignement other)
    {
        return (Hostile & other.Group) != 0;
    }
}