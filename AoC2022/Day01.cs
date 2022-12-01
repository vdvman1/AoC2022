using System.Runtime.InteropServices;

namespace AoC2022;

public class Day01 : BaseDay
{
    private readonly int[] ElfRationTotals;

    public Day01()
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

    public override ValueTask<string> Solve_1()
    {
        int maxCalories = 0;
        var totals = ElfRationTotals.AsSpan();
        for (int i = 0; i < totals.Length; i++)
        {
            if (totals[i] > maxCalories)
            {
                maxCalories = totals[i];
            }
        }
        return new(maxCalories.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        const int count = 3;
        Span<int> topThree = stackalloc int[count];
        var totals = ElfRationTotals.AsSpan();
        totals[..count].CopyTo(topThree);
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

        totals = totals.Slice(count);
        for (int i = 0; i < totals.Length; i++)
        {
            var total = totals[i];
            for (int j = 0; j < count; j++)
            {
                if (total < topThree[j]) continue;

                for (int k = count - 1; k > j; k--)
                {
                    topThree[k] = topThree[k - 1];
                }

                topThree[j] = total;
                break;
            }
        }

        return new((topThree[0] + topThree[1] + topThree[2]).ToString());
    }
}
