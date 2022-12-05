namespace AoC2022;

public partial class Day02 : DayBase
{
    const byte DrawPoints = 3;
    const byte WinPoints = 6;

    const int Rock = 0b00;     // 0
    const int Paper = 0b01;    // 1
    const int Scissors = 0b10; // 2
    // Unused: 0b11

    const int Lose = 0;
    const int Draw = 1;
    const int Win = 2;

    private byte[] Rows = null!;

    [Benchmark]
    public override void ParseData()
    {
        var chars = Contents;
        var rows = new List<byte>();
        for (int i = 0; i < chars.Length; i += 4)
        {
            var opponent = chars[i] - 'A';
            var response = chars[i + 2] - 'X';

            rows.Add((byte)(opponent << 2 | response));
        }

        Rows = rows.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        ReadOnlySpan<byte> PointsLookup = new byte[]
        {
                                                                //                  opponent - response =  0 -> 3   + 3 =  3   * 3 =  9     % 5 = 4
                                                                //                  opponent - response = -1 -> 6   + 3 =  2   * 3 =  6     % 5 = 1
                                                                //                  opponent - response = -2 -> 0   + 3 =  1   * 3 =  3     % 5 = 3
                                                                //                  opponent - response =  1 -> 0   + 3 =  4   * 3 = 12     % 5 = 2
                                                                //                  opponent - response =  2 -> 6   + 3 =  5   * 3 = 15     % 5 = 0

            /* Rock    , Rock     */ DrawPoints + Rock + 1,     // 3 + 0 + 1 = 4    opponent - response = 0 - 0 =  0
            /* Rock    , Paper    */ WinPoints + Paper + 1,     // 6 + 1 + 1 = 8    opponent - response = 0 - 1 = -1
            /* Rock    , Scissors */ 0 + Scissors + 1,          //     2 + 1 = 3    opponent - response = 0 - 2 = -2
            /* Rock    , Unused   */ 0,
            /* Paper   , Rock     */ 0 + Rock + 1,              //     0 + 1 = 1    opponent - response = 1 - 0 =  1
            /* Paper   , Paper    */ DrawPoints + Paper + 1,    // 3 + 1 + 1 = 5    opponent - response = 1 - 1 =  0
            /* Paper   , Scissors */ WinPoints + Scissors + 1,  // 6 + 2 + 1 = 9    opponent - response = 1 - 2 = -1
            /* Paper   , Unused   */ 0,
            /* Scissors, Rock     */ WinPoints + Rock + 1,      // 6 + 0 + 1 = 7    opponent - response = 2 - 0 =  2
            /* Scissors, Paper    */ 0 + Paper + 1,             //     1 + 1 = 2    opponent - response = 2 - 1 =  1
            /* Scissors, Scissors */ DrawPoints + Scissors + 1, // 3 + 2 + 1 = 6    opponent - response = 2 - 2 =  0
            /* Scissors, Unused   */ 0,
        };

        int score = 0;
        var rowArray = Rows;
        ref var rows = ref MemoryMarshal.GetArrayDataReference(rowArray);
        for (int i = 0; i < rowArray.Length; ++i)
        {
            var row = Unsafe.Add(ref rows, i);
            score += PointsLookup[row];
        }
        return score.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        ReadOnlySpan<byte> PointsLookup = new byte[]
        {
            /* Rock    , Lose   */ 0 + Scissors + 1,
            /* Rock    , Draw   */ DrawPoints + Rock + 1,
            /* Rock    , Win    */ WinPoints + Paper + 1,
            /* Rock    , Unused */ 0,
            /* Paper   , Lose   */ 0 + Rock + 1,
            /* Paper   , Draw   */ DrawPoints + Paper + 1,
            /* Paper   , Win    */ WinPoints + Scissors + 1,
            /* Paper   , Unused */ 0,
            /* Scissors, Lose   */ 0 + Paper + 1,
            /* Scissors, Draw   */ DrawPoints + Scissors + 1,
            /* Scissors, Win    */ WinPoints + Rock + 1,
            /* Scissors, Unused */ 0,
        };

        int score = 0;
        var rowArray = Rows;
        ref var rows = ref MemoryMarshal.GetArrayDataReference(rowArray);
        for (int i = 0; i < rowArray.Length; ++i)
        {
            var row = Unsafe.Add(ref rows, i);
            score += PointsLookup[row];
        }
        return score.ToString();
    }
}
