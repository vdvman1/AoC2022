namespace AoC2022;

public partial class Day09 : DayBase
{
    private enum Direction : byte
    {
        Left,
        Right,
        Up,
        Down
    }

    public (sbyte x, sbyte y)[] Steps = null!;

    [Benchmark]
    public override void ParseData()
    {
        ReadOnlySpan<byte> chars = Contents;

        const string dirTxt = "D ";
        const string minStepTxt = dirTxt + "1\n";

        var steps = new List<(sbyte x, sbyte y)>((chars.Length - 1) / minStepTxt.Length + 1); // Overestimate number of lines by assuming all lines have only one digit and doing a ceiling division

        for (int i = 0; i < chars.Length; i++)
        {
            var dir = chars[i];
            i += dirTxt.Length;

            int offset = chars[i] - '0';
            ++i;
            if (chars[i] != (byte)'\n')
            {
                offset = 10 * offset + chars[i] - '0';
                ++i;
            }

            steps.Add(dir switch
            {
                (byte)'R' => ((sbyte)offset, 0),
                (byte)'L' => ((sbyte)-offset, 0),
                (byte)'U' => (0, (sbyte)offset),
                _ /* D */ => (0, (sbyte)-offset)
            });
        }

        Steps = steps.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        (int x, int y) head = (0, 0);
        var tail = head;

        var visited = new HashSet<(int x, int y)>
        {
            tail
        };

        foreach ((sbyte x, sbyte y) step in Steps)
        {
            head.x += step.x;
            head.y += step.y;

            var xDiff = head.x - tail.x;
            var yDiff = head.y - tail.y;

            if (xDiff > 1)
            {
                for (int i = 1; i < xDiff; i++)
                {
                    visited.Add((tail.x + i, head.y));
                }

                tail = (head.x - 1, head.y);
            }
            else if (xDiff < -1)
            {
                for (int i = -1; i > xDiff; i--)
                {
                    visited.Add((tail.x + i, head.y));
                }
                tail = (head.x + 1, head.y);
            }
            else if (yDiff > 1)
            {
                for (int i = 1; i < yDiff; i++)
                {
                    visited.Add((head.x, tail.y + i));
                }
                tail = (head.x, head.y - 1);
            }
            else if (yDiff < -1)
            {
                for (int i = -1; i > yDiff; i--)
                {
                    visited.Add((head.x, tail.y + i));
                }
                tail = (head.x, head.y + 1);
            }
        }

        return visited.Count.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}