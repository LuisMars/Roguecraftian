using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Components;

public class Inventory
{
    public int CurrentIndex { get; private set; } = 0;
    public Item[] Items { get; set; } = new Item[8];

    public void Add(Item item)
    {
        for (var i = 0; i < Items.Length; i++)
        {
            if (Items[i] is not null)
            {
                continue;
            }
            Items[i] = item;
            break;
        }
    }

    public void SelectNext()
    {
        CurrentIndex++;
        CurrentIndex %= Items.Length;
    }

    public void SelectPrev()
    {
        CurrentIndex--;
        if (CurrentIndex < 0)
        {
            CurrentIndex = Items.Length - 1;
        }
    }

    public Item? UseCurrentItem()
    {
        var item = Items[CurrentIndex];
        if (item is null)
        {
            return null;
        }
        Items[CurrentIndex] = null;

        return item;
    }
}