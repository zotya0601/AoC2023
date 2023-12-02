using System.Collections.Generic;
using System.Linq;
using AoC2;
using Xunit;

namespace AoC2Test;

// Tests should not be written like this... Maybe next day
// For now, good enough to test CI
public class Tests
{
    private const string input = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

    private readonly Bag _bag;
    private readonly IEnumerable<Game> _games;
    
    public Tests()
    {
        _bag = new Bag(12, 13, 14);
        _games = input.Split("\n")
            .Select(line => new Game(line));
    }
    
    [Fact]
    public void Day2Part1_Examples_Pass()
    {
        var sum = _games.Where(game => game.PossibleWithBag(_bag))
            .Sum(game => game.Id);
        
        Assert.Equal(8, sum);
    }

    [Fact]
    public void Day2Part2_Examples_Pass()
    {
        long power = _games.Sum(game => game.PowerOfCubes);
        Assert.Equal(2286, power);
    }
}