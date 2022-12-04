namespace AoC2022;

public abstract class DayBase
{
    public readonly string InputFilePath;
    public readonly string Day;

    public DayBase(string day)
    {
        InputFilePath = string.Concat("Inputs/", day, ".txt");
        Day = day;
        ParseData();
    }


    public abstract void ParseData();

    public abstract string Solve1();

    public abstract string Solve2();
}
