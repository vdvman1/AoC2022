namespace AoC2022;

public abstract class DayBase
{
    public DayBase() => ParseData();

    public abstract void ParseData();

    public abstract string Solve1();

    public abstract string Solve2();
}
