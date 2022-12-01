namespace AoC2022;

public class Day01 : DayBase
{
    private int[] ElfRationTotals = Array.Empty<int>();

    public Day01() : base("01") { }

    [Benchmark]
    public override void LoadData()
    {
        var lines = File.ReadAllLines(InputFilePath);
        var totals = new List<int>();
        var sum = 0;
        foreach (var line in lines)
        {
            if (line.Length == 0)
            {
                totals.Add(sum);
                sum = 0;
            }
            else
            {
                sum += int.Parse(line);
            }
        }
        totals.Add(sum);
        ElfRationTotals = totals.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        int maxCalories = 0;
        foreach (int rations in ElfRationTotals)
        {
            maxCalories = Math.Max(maxCalories, rations);
        }

        return maxCalories.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        const int count = 3;
        var topThree = ElfRationTotals[..count];
        if (topThree[0] >= topThree[1])
        {
            // 0 >= 1
            if (topThree[1] >= topThree[2])
            {
                // 0 >= 1 >= 2
                // Already sorted
            }
            else
            {
                // 2 > 1
                if (topThree[0] >= topThree[2])
                {
                    // 0 >= 2 > 1
                    (topThree[1], topThree[2]) = (topThree[2], topThree[1]);
                }
                else
                {
                    // 2 > 0 >= 1
                    (topThree[0], topThree[1], topThree[2]) = (topThree[2], topThree[0], topThree[1]);
                }
            }
        }
        else
        {
            // 1 > 0
            if (topThree[1] >= topThree[2])
            {
                // 1 >= 2
                if (topThree[0] >= topThree[2])
                {
                    // 1 > 0 >= 2
                    (topThree[0], topThree[1]) = (topThree[1], topThree[0]);
                }
                else
                {
                    // 1 >= 2 > 0
                    (topThree[0], topThree[1], topThree[2]) = (topThree[1], topThree[2], topThree[0]);
                }
            }
            else
            {
                // 2 > 1 > 0
                (topThree[0], topThree[2]) = (topThree[2], topThree[0]);
            }
        }

        for (int i = count; i < ElfRationTotals.Length; i++)
        {
            var calories = ElfRationTotals[i];
            if (calories > topThree[2])
            {
                if (calories > topThree[1])
                {
                    topThree[2] = topThree[1];
                    if (calories > topThree[0])
                    {
                        topThree[1] = topThree[0];
                        topThree[0] = calories;
                    }
                    else
                    {
                        topThree[1] = calories;
                    }
                }
                else
                {
                    topThree[2] = calories;
                }
            }
        }

        return (topThree[0] + topThree[1] + topThree[2]).ToString();
    }
}
