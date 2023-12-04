using System.Text;

namespace AoC3;

public static class StringExtensions
{
    public static char SetAsAlreadyRead(char character) => (char)(character | 0b1000_0000);
    public static char SetAsNotRead(char character) => (char)(character & 0b0111_1111);
    
    public static int GetNumberAroundCoordinates(int x, List<char> line)
    {
        int number = 0;
        int xStart = x;
        while (xStart >= 0)
        {
            char ch = line[xStart];
            if(!char.IsDigit(ch)) break;
            xStart--;
        }

        xStart++;
        StringBuilder sbNumber = new();
        for (int i = xStart; i < line.Count; i++)
        {
            char ch = line[i];
            if (!char.IsDigit(ch)) break;
            sbNumber.Append(ch);
            line[i] = '.';
        }

        string sNumber = sbNumber.ToString();
        number = int.Parse(sNumber);
        return number;
    }
    
    public static List<int> GetGoodNumbers(this List<List<char>> wholeFile, 
        Func<char, bool> shouldJump,
        Func<List<int>, bool> insertionPredicate,
        Func<List<int>, List<int>>? modifyGoodNumbers)
    {
        List<int> numbers = new();
        for (int y = 0; y < wholeFile.Count; y++)
        {
            List<char> line = wholeFile[y];
            for (int x = 0; x < line.Count; x++)
            {
                char ch = line[x];
                if(shouldJump(ch)) continue;
                
                int yStart = Math.Max(y - 1, 0);
                int xStart = Math.Max(x - 1, 0);
                int yEnd = Math.Min(y + 1, wholeFile.Count - 1);
                int xEnd = Math.Min(x + 1, line.Count - 1);

                List<int> goodNumbers = new();
                for (int i = yStart; i <= yEnd; i++)
                {
                    for (int j = xStart; j <= xEnd; j++)
                    {
                        char c = wholeFile[i][j];
                        if (char.IsDigit(c))
                            goodNumbers.Add(GetNumberAroundCoordinates(j, wholeFile[i]));
                    }
                }
                if(insertionPredicate(goodNumbers))
                    numbers.AddRange(modifyGoodNumbers?.Invoke(goodNumbers) ?? goodNumbers);
                else
                {
                    for (int i = yStart; i <= yEnd; i++)
                    {
                        for (int j = xStart; j <= xEnd; j++)
                        {
                            SetAsNotRead(wholeFile[j][i]);
                        }
                    }
                }
            }
        }
        return numbers;
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");
        List<int> goodNumbers = lines
            .Select(line => 
                line.Select(ch => ch).ToList())
            .ToList()
            .GetGoodNumbers(ch => char.IsDigit(ch) || ch == '.', _ => true, null);
        
        Console.WriteLine(goodNumbers.Sum());
        
        goodNumbers = lines
            .Select(line => 
                line.Select(ch => ch).ToList())
            .ToList()
            .GetGoodNumbers(
                ch => char.IsDigit(ch) || ch == '.', 
                gNs => gNs.Count == 2, ints => new(){ ints[0] * ints[1] });

        Console.WriteLine(goodNumbers.Sum());
    }
}