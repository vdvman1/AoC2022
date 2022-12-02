using AoC2022;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using System.Text;

const bool benchmark = true;
if (benchmark)
{
    BenchmarkRunner.Run<Day02>(ManualConfig.CreateMinimumViable()
        .AddJob(Job
            .MediumRun
            .WithToolchain(InProcessNoEmitToolchain.Instance)
        )
    );
}

string part1 = "Part 1";
string part2 = "Part 2";
int sol1MaxLen = part1.Length;
int sol2MaxLen = part2.Length;
SortedList<string, (string sol1, string sol2)> solutions = new();
foreach (var cls in typeof(DayBase).Assembly.GetTypes().Where(cls => cls.IsSubclassOf(typeof(DayBase))))
{
    var day = (DayBase)Activator.CreateInstance(cls)!;
    string sol1, sol2;
    try
    {
        sol1 = day.Solve1();
    }
    catch (Exception e)
    {
        sol1 = e.Message;
    }
    try
    {
        sol2 = day.Solve2();
    }
    catch (Exception e)
    {
        sol2 = e.Message;
    }
    solutions.Add(day.Day, (sol1, sol2));
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

var builder = new StringBuilder(1 + (1 + dayTitle.Length + 1) + 1 + (1 + sol1MaxLen + 1) + 1 + (1 + sol2MaxLen + 1) + 1);

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
var days = solutions.Keys;
var results = solutions.Values;
void WriteSolution(int i)
{
    var day = days[i];
    var (sol1, sol2) = results[i];
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