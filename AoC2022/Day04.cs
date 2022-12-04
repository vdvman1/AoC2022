namespace AoC2022;

public class Day04 : DayBase
{
    public Day04() : base("04") { }

    private record struct Range(int Start, int End);
    private (Range A, Range B)[] Ranges = null!;

    [Benchmark]
    public override void LoadData()
    {
        var lines = File.ReadAllLines(InputFilePath);
        var ranges = new List<(Range A, Range B)>(lines.Length);

        foreach (var line in lines)
        {
            var parts = line.Split('-', ',');
            ranges.Add((
                new(int.Parse(parts[0]), int.Parse(parts[1])),
                new(int.Parse(parts[2]), int.Parse(parts[3]))
            ));
        }

        Ranges = ranges.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        int count = 0;

        foreach (var (a, b) in Ranges)
        {
            if (a.Start == b.Start || a.End == b.End)
            {
                ++count;
            }
            else if (a.Start < b.Start)
            {
                if (b.End < a.End)
                {
                    ++count;
                }
            }
            else // b.Start < a.Start
            {
                if (a.End < b.End)
                {
                    ++count;
                }
            }
        }

        return count.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        int count = 0;

        foreach (var (a, b) in Ranges)
        {
            if (a.Start <= b.Start)
            {
                if (b.Start <= a.End)
                {
                    ++count;
                }
            }
            else // b.Start < a.Start
            {
                if (a.Start <= b.End)
                {
                    ++count;
                }
            }
        }

        return count.ToString();
    }
}
