namespace AoC2022;

public partial class Day03 : DayBase
{
    private (ulong CompartmentA, ulong CompartmentB)[] Sacks = null!;

    [Benchmark]
    public override void ParseData()
    {
        var chars = Contents;
        var sacks = new List<(ulong CompartmentA, ulong CompartmentB)>();

        // My input file contains between 16 and 16+32 characters for each line, and Vector256<byte>.Count == 32
        int vecEnd = chars.Length - Vector256<byte>.Count - 16;
        int i = 0;
        while (i <= vecEnd)
        {
            int end = i + 16;
            var charVec = Vector256.LoadUnsafe(ref Unsafe.AsRef(in chars[end]));
            var isNewlineVec = Vector256.Equals(charVec, Vector256.Create((byte)'\n'));
            var isNewlineBits = isNewlineVec.ExtractMostSignificantBits();
            end += BitOperations.TrailingZeroCount(isNewlineBits);

            int length = (end - i) >> 1;
            ulong compartmentA = 0;

            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentA |= (1ul << index);
                ++i;
            }

            ulong compartmentB = 0;
            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentB |= (1ul << index);
                ++i;
            }

            sacks.Add((compartmentA, compartmentB));
            ++i;
        }

        // Attempting to use Vector128 made no difference to performance
        
        while (i < chars.Length)
        {
            int end = i + 16;
            while (chars[end] != '\n')
            {
                ++end;
            }

            int length = (end - i) >> 1;
            ulong compartmentA = 0;
            ulong compartmentB = 0;

            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentA |= (1ul << index);
                ++i;
            }

            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentB |= (1ul << index);
                ++i;
            }

            sacks.Add((compartmentA, compartmentB));
            ++i;
        }

        Sacks = sacks.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        int sum = 0;
        foreach (var (a, b) in Sacks)
        {
            var common = a & b;
            int value = BitOperations.TrailingZeroCount(common);
            sum += value + 1;
        }

        return sum.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        int sum = 0;

        for (int i = 0; i < Sacks.Length; i += 3)
        {
            var (a, b) = Sacks[i];
            var set = a | b;

            (a, b) = Sacks[i + 1];
            set &= a | b;

            (a, b) = Sacks[i + 2];
            set &= a | b;

            int value = BitOperations.TrailingZeroCount(set);
            sum += value + 1;
        }

        return sum.ToString();
    }
}
