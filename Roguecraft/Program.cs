using Microsoft.Xna.Framework;
using Roguecraft.Viewers;
using System;

namespace Roguecraft;

public static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            using var main = new Main();
            main.Run();
            return;
        }

        using Game game = args[0] switch
        {
            "dungeonViewer" => new DungeonViewer(),
            "roomDecorator" => new RoomDecoratorViewer(),
            _ => throw new ArgumentException($"{args[0]} is not supported")
        };
        game.Run();
    }
}