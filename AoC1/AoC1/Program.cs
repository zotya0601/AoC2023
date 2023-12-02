using System.Text;
using System.Text.RegularExpressions;

namespace AoC1;

static class Methods
{
    private static readonly Regex Regex = new("(one)|(two)|(three)|(four)|(five)|(six)|(seven)|(eight)|(nine)|0|1|2|3|4|5|6|7|8|9");
    private static readonly Regex ReverseRegex = new("(eno)|(owt)|(eerht)|(ruof)|(evif)|(xis)|(neves)|(thgie)|(enin)|0|1|2|3|4|5|6|7|8|9");
    private static readonly Dictionary<string, int> Numbers = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };
    
    public static int GetCalibration(this string line)
    {
        var matches = Regex.Matches(line);
        string reversedLine = new string(line.Reverse().ToArray());
        var reverseMatches = ReverseRegex.Matches(reversedLine);
        
        string firstString = matches.First().Value;
        string lastString = reverseMatches.First().Value;
        lastString = new string(lastString.Reverse().ToArray());

        int first = firstString.Length == 1 ? int.Parse(firstString) : Numbers[firstString];
        int last = lastString.Length == 1 ? int.Parse(lastString) : Numbers[lastString];

        return first * 10 + last;
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        var lines = File.ReadAllLines("./input.txt");
        var sum = lines.Sum(l => l.GetCalibration());
        Console.WriteLine(sum);
    }
}