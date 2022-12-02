namespace AoC2022;

public class Day01 : DayBase
{
    private int[] ElfRationTotals = Array.Empty<int>();

    public Day01() : base("01") { }

    [Benchmark]
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void LoadData()
    {
        var chars = File.ReadAllBytes(InputFilePath);
        var totals = new List<int>();
        int total = 0;
        int i = 0;
        do
        {
            var charsVec = Unsafe.ReadUnaligned<int>(ref chars[i]);
            const byte zero = (byte)'0';
            const int zeroVec = zero | (zero << 8) | (zero << 16) | (zero << 24);
            charsVec -= zeroVec;
            ref byte digit = ref Unsafe.As<int, byte>(ref charsVec);
            int value =
                (10 * 10 * 10 * digit)
                + (10 * 10 * Unsafe.Add(ref digit, 1))
                + (10 * Unsafe.Add(ref digit, 2))
                + Unsafe.Add(ref digit, 3);

            if (i + 4 == chars.Length)
            {
                totals.Add(total + value);
                break;
            }

            if (chars[i + 4] == '\n')
            {
                if (i + 5 == chars.Length)
                {
                    totals.Add(total + value);
                    break;
                }

                if (chars[i + 5] == '\n')
                {
                    totals.Add(total + value);
                    i += 6;
                    total = 0;
                    continue;
                }

                total += value;
                i += 5;
                continue;
            }

            total += 10 * value + chars[i + 4] - '0';
            if (i + 5 == chars.Length)
            {
                totals.Add(total);
                break;
            }

            if (chars[i + 5] == '\n')
            {
                if (i + 6 == chars.Length)
                {
                    totals.Add(total);
                    break;
                }

                if (chars[i + 6] == '\n')
                {
                    totals.Add(total);
                    total = 0;
                    i += 7;
                    continue;
                }

                i += 6;
                continue;
            }

            i += 5;
        } while (i < chars.Length);
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
