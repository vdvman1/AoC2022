namespace AoC2022;

public partial class Day06 : DayBase
{
    public override void ParseData() { }

    [Benchmark]
    public override string Solve1()
    {
        ReadOnlySpan<byte> chars = Contents;
        int start = 0;
        int i = 1;
        do
        {
            for (int j = i - 1; j >= start; j--)
            {
                if (chars[i] == chars[j])
                {
                    start = j + 1;
                    goto NextStart;
                }
            }

            if (i - start == 3)
            {
                return (i + 1).ToString();
            }

            NextStart:
            ++i;
        } while (i < chars.Length);

        return "No start markers found";
    }

    [Benchmark]
    public override string Solve2()
    {
        ReadOnlySpan<byte> chars = Contents;
        Span<int> seenIndex = stackalloc int['z' - 'a' + 1];
        seenIndex.Fill(-1);

        int start = 0;
        seenIndex[chars[0] - 'a'] = 0;
        int i = 1;
        do
        {
            var idx = chars[i] - 'a';
            var offset = seenIndex[idx];
            if (offset >= 0)
            {
                while (start <= offset)
                {
                    seenIndex[chars[start] - 'a'] = -1;
                    ++start;
                }
            }
            else if (i - start == 13)
            {
                return (i + 1).ToString();
            }

            seenIndex[idx] = i;
            ++i;
        } while (i < chars.Length);

        return "No start markers found";
    }
}
