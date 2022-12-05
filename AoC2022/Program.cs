using System.Text;

namespace AoC2022;

public partial class Program
{
    public static void Main()
    {
        BenchmarkLatest();
        //BenchmarkAll();

        string part1 = "Part 1";
        string part2 = "Part 2";
        int sol1MaxLen = part1.Length;
        int sol2MaxLen = part2.Length;
        var solutions = new List<(string day, string sol1, string sol2)>(Days.Count);
        foreach (var (num, instance) in Days)
        {
            string sol1, sol2;
            try
            {
                sol1 = instance.Solve1();
            }
            catch (Exception e)
            {
                sol1 = e.Message;
            }
            try
            {
                sol2 = instance.Solve2();
            }
            catch (Exception e)
            {
                sol2 = e.Message;
            }
            solutions.Add((num, sol1, sol2));
            sol1MaxLen = Math.Max(sol1MaxLen, sol1.Length);
            sol2MaxLen = Math.Max(sol2MaxLen, sol2.Length);
        }

        string dayTitle = "Day";

        char lineH = '─';
        char lineV = '│';
        char cornerTL = '┌';
        char cornerTR = '┐';
        char cornerBL = '└';
        char cornerBR = '┘';
        char joinL = '├';
        char joinR = '┤';
        char joinT = '┬';
        char joinB = '┴';
        char joinC = '┼';

        var builder = new StringBuilder(1 + 1 + dayTitle.Length + 1 + 1 + 1 + sol1MaxLen + 1 + 1 + 1 + sol2MaxLen + 1 + 1);

        builder.Append(lineH, 1 + dayTitle.Length + 1);
        var lineDay = builder.ToString();
        builder.Clear();

        builder.Append(lineH, 1 + sol1MaxLen + 1);
        var lineSol1 = builder.ToString();
        builder.Clear();

        builder.Append(lineH, 1 + sol2MaxLen + 1);
        var lineSol2 = builder.ToString();

        builder
            .Clear()
            .Append(cornerTL)
            .Append(lineDay)
            .Append(joinT)
            .Append(lineSol1)
            .Append(joinT)
            .Append(lineSol2)
            .Append(cornerTR);
        Console.WriteLine(builder.ToString());

        builder
            .Clear()
            .Append(lineV)
            .Append(' ')
            .Append(dayTitle)
            .Append(' ')
            .Append(lineV)
            .Append(' ')
            .Append(part1)
            .Append(' ', sol1MaxLen + 1 - part1.Length)
            .Append(lineV)
            .Append(' ')
            .Append(part2)
            .Append(' ', sol2MaxLen + 1 - part2.Length)
            .Append(lineV);
        Console.WriteLine(builder.ToString());

        builder
            .Clear()
            .Append(joinL)
            .Append(lineDay)
            .Append(joinC)
            .Append(lineSol1)
            .Append(joinC)
            .Append(lineSol2)
            .Append(joinR);
        var separator = builder.ToString();
        Console.WriteLine(separator);

        var end = solutions.Count - 1;
        void WriteSolution(int i)
        {
            var (day, sol1, sol2) = solutions[i];
            builder
               .Clear()
               .Append(lineV)
               .Append(' ')
               .Append(day)
               .Append(' ', dayTitle.Length + 1 - day.Length)
               .Append(lineV)
               .Append(' ')
               .Append(sol1)
               .Append(' ', sol1MaxLen + 1 - sol1.Length)
               .Append(lineV)
               .Append(' ')
               .Append(sol2)
               .Append(' ', sol2MaxLen + 1 - sol2.Length)
               .Append(lineV);
            Console.WriteLine(builder.ToString());
        }

        for (int i = 0; i < end; i++)
        {
            WriteSolution(i);
            Console.WriteLine(separator);
        }

        WriteSolution(end);

        builder
            .Clear()
            .Append(cornerBL)
            .Append(lineDay)
            .Append(joinB)
            .Append(lineSol1)
            .Append(joinB)
            .Append(lineSol2)
            .Append(cornerBR);
        Console.WriteLine(builder.ToString());
    }
}