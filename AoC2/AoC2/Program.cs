﻿namespace AoC2;

internal record Bag(int Red, int Green, int Blue);

internal class Game
{
    // Max amount for each
    private readonly int _red;
    private readonly int _green;
    private readonly int _blue;

    public int Id { get; }

    internal Game(string line)
    {
        Id = int.Parse(line.Split(":")[0].Split(" ")[1]);
        
        var res = line.Split(":")[1].Split(";").AsEnumerable()
            .SelectMany(grab => grab.Split(","))
            .Select(part => part.Trim().Split(" "))
            .Select(parts => new { Amount = int.Parse(parts[0]), Color = parts[1] }).ToList()
            .GroupBy(obj => obj.Color)
            .Select(grouping => new { Color = grouping.Key, Amount = grouping.Max(obj => obj.Amount) })
            .ToLookup(obj => obj.Color);
        
        _red = res["red"].Single().Amount;
        _green = res["green"].Single().Amount;
        _blue = res["blue"].Single().Amount;
    }

    public bool PossibleWithBag(Bag bag) => bag.Red >= _red && bag.Green >= _green && bag.Blue >= _blue;
    public int GetPowerOfDies() => _red * _green * _blue;
}

internal static class Program
{
    public static void Main()
    {
        Bag bag = new Bag(12, 13, 14);
        
        var games = File.ReadLines("input.txt")
            .Select(line => new Game(line)).ToList();
        
        var sum = games.Where(game => game.PossibleWithBag(bag))
            .Sum(game => game.Id);
        Console.WriteLine($"Sum of IDs is {sum}");

        long power = games.Sum(game => game.GetPowerOfDies());
        Console.WriteLine($"Power of dies is {power}");
    }
}