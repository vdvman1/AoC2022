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
                    break;
                }
            }

            if (i - start == 3)
            {
                return (i + 1).ToString();
            }

            ++i;
        } while (i < chars.Length);

        return "No start markers found";
    }

    [Benchmark]
    public override string Solve2()
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
                    break;
                }
            }

            if (i - start == 13)
            {
                return (i + 1).ToString();
            }

            ++i;
        } while (i < chars.Length);

        return "No start markers found";
    }
}
