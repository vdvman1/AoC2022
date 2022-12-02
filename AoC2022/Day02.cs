namespace AoC2022;

public class Day02 : DayBase
{
    public Day02() : base("02") { }

    const int DrawPoints = 3;
    const int WinPoints = 6;

    private enum ThrowChoice : byte
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }

    private enum Response : byte
    {
        Rock = 0,
        Lose = Rock,
        Paper = 1,
        Draw = Paper,
        Scissors = 2,
        Win = Scissors
    }

    private record struct Row(ThrowChoice Opponent, Response Response);

    private Row[] Rows = null!;

    [Benchmark]
    public override void LoadData()
    {
        var lines = File.ReadAllLines(InputFilePath);
        var rows = new List<Row>(lines.Length);
        foreach (var line in lines)
        {
            var opponent = line[0] - 'A';
            var response = line[2] - 'X';
            rows.Add(new((ThrowChoice)opponent, (Response)response));
        }
        Rows = rows.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        int score = 0;
        foreach (var (opponent, response) in Rows)
        {
            score += (byte)response + 1;
            switch (opponent)
            {
                case ThrowChoice.Rock:
                    switch (response)
                    {
                        case Response.Rock:
                            score += DrawPoints;
                            break;
                        case Response.Paper:
                            score += WinPoints;
                            break;
                    }
                    break;
                case ThrowChoice.Paper:
                    switch (response)
                    {
                        case Response.Paper:
                            score += DrawPoints;
                            break;
                        case Response.Scissors:
                            score += WinPoints;
                            break;
                    }
                    break;
                case ThrowChoice.Scissors:
                    switch (response)
                    {
                        case Response.Rock:
                            score += WinPoints;
                            break;
                        case Response.Scissors:
                            score += DrawPoints;
                            break;
                    }
                    break;
            }
        }
        return score.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        int score = 0;
        foreach (var (opponent, response) in Rows)
        {
            switch (opponent)
            {
                case ThrowChoice.Rock:
                    switch (response)
                    {
                        case Response.Lose:
                            score += (byte)ThrowChoice.Scissors + 1;
                            break;
                        case Response.Draw:
                            score += DrawPoints + (byte)ThrowChoice.Rock + 1;
                            break;
                        case Response.Win:
                            score += WinPoints + (byte)ThrowChoice.Paper + 1;
                            break;
                    }
                    break;
                case ThrowChoice.Paper:
                    switch (response)
                    {
                        case Response.Lose:
                            score += (byte)ThrowChoice.Rock + 1;
                            break;
                        case Response.Draw:
                            score += DrawPoints + (byte)ThrowChoice.Paper + 1;
                            break;
                        case Response.Win:
                            score += WinPoints + (byte)ThrowChoice.Scissors + 1;
                            break;
                    }
                    break;
                case ThrowChoice.Scissors:
                    switch (response)
                    {
                        case Response.Lose:
                            score += (byte)ThrowChoice.Paper + 1;
                            break;
                        case Response.Draw:
                            score += DrawPoints + (byte)ThrowChoice.Scissors + 1;
                            break;
                        case Response.Win:
                            score += WinPoints + (byte)ThrowChoice.Rock + 1;
                            break;
                    }
                    break;
            }
        }

        return score.ToString();
    }
}
