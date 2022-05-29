using System;

namespace Roguecraft;

public static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        using var game = new Main();
        game.Run();
    }
}