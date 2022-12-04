namespace AoC2022;

public class Day03 : DayBase
{
    public Day03() : base("03") { }

    private byte[][] Sacks = null!;

    [Benchmark]
    public override void LoadData()
    {
        var lines = File.ReadAllLines(InputFilePath);
        var sacks = new List<byte[]>(lines.Length);
        foreach (var line in lines)
        {
            var sack = new byte[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                sack[i] = (byte)(c > 'Z' ? c - 'a' + 1 : c - 'A' + 27);
            }

            sacks.Add(sack);
        }

        Sacks = sacks.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        int sum = 0;
        foreach (var sack in Sacks)
        {
            var length = sack.Length >> 1;
            for (int i = 0; i < length; i++)
            {
                if (Array.IndexOf(sack, sack[i], length) >= 0)
                {
                    sum += sack[i];
                    break;
                }
            }
        }

        return sum.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        int sum = 0;

        for (int i = 0; i < Sacks.Length; i += 3)
        {
            var set = new HashSet<byte>(Sacks[i]);
            set.IntersectWith(Sacks[i + 1]);
            set.IntersectWith(Sacks[i + 2]);
            sum += set.Single();
        }

        return sum.ToString();
    }
}
