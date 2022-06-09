﻿namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ChairRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'F', 'F', 'F' },
        { '*', 'F', '*' },
        { '*', 'T', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'F', 'F', 'F' },
        { '*', 'C', '*' },
        { '*', 'T', '*' },
    };
}