namespace AoC2022;

public class Day01 : BaseDay
{
    private readonly List<List<int>> ElfRations = new();

    public Day01()
    {
        var lines = File.ReadAllLines(InputFilePath);
        var rations = new List<int>();
        foreach (var line in lines)
        {
            if (line.Length == 0)
            {
                ElfRations.Add(rations);
                rations = new();
            }
            else
            {
                rations.Add(int.Parse(line));
            }
        }
        ElfRations.Add(rations);
    }

    public override ValueTask<string> Solve_1()
    {
        int maxCalories = 0;
        foreach (var rations in ElfRations)
        {
            maxCalories = Math.Max(maxCalories, rations.Sum());
        }
        return new(maxCalories.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}
