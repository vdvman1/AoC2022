namespace AoC2022;

public abstract class DayBase
{
    public readonly string Day;

    public DayBase(string day)
    {
        Day = day;
        ParseData();
    }

    public abstract void ParseData();

    public abstract string Solve1();

    public abstract string Solve2();
}
