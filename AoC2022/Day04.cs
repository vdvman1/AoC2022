namespace AoC2022;

public partial class Day04 : DayBase
{
    private (byte StartA, byte EndA, byte StartB, byte EndB)[] Ranges = null!;

    [Benchmark]
    public override void ParseData()
    {
        var chars = Contents;
        var ranges = new List<(byte StartA, byte EndA, byte StartB, byte EndB)>();
        int i = 0;
        while (i < chars.Length)
        {
            int startA = chars[i] - '0';
            if (chars[i + 1] == '-')
            {
                i += 2;
            }
            else
            {
                startA = 10*startA + chars[i + 1] - '0';
                i += 3;
            }

            int endA = chars[i] - '0';
            if (chars[i + 1] == ',')
            {
                i += 2;
            }
            else
            {
                endA = 10 * endA + chars[i + 1] - '0';
                i += 3;
            }

            int startB = chars[i] - '0';
            if (chars[i + 1] == '-')
            {
                i += 2;
            }
            else
            {
                startB = 10 * startB + chars[i + 1] - '0';
                i += 3;
            }

            int endB = chars[i] - '0';
            if (chars[i + 1] == '\n')
            {
                i += 2;
            }
            else
            {
                endB = 10 * endB + chars[i + 1] - '0';
                i += 3;
            }

            if (startB <= startA)
            {
                ranges.Add((
                    (byte)startB, (byte)endB,
                    (byte)startA, (byte)endA
                ));
            }
            else
            {
                ranges.Add((
                    (byte)startA, (byte)endA,
                    (byte)startB, (byte)endB
                ));
            }
        }

        Ranges = ranges.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        int count = 0;

        var ranges = Ranges;
        for (int i = 0; i < ranges.Length; i++)
        {
            var (startA, endA, startB, endB) = ranges[i];
            if (startA == startB || endB <= endA)
            {
                ++count;
            }
        }

        return count.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        int count = 0;

        var ranges = Ranges;
        for (int i = 0; i < ranges.Length; i++)
        {
            var (_, endA, startB, _) = ranges[i];
            if (startB <= endA)
            {
                ++count;
            }
        }

        return count.ToString();
    }
}
