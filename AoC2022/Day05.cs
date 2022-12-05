namespace AoC2022;

public partial class Day05 : DayBase
{
    private readonly struct Instruction
    {
        public readonly byte Count;
        public readonly byte From;
        public readonly byte To;

        public Instruction(byte count, byte from, byte to)
        {
            Count = count;
            From = from;
            To = to;
        }
    }

    private Instruction[] Instructions = null!;
    private List<byte>[] StackLists = null!;

    [Benchmark]
    public override void ParseData()
    {
        var chars = Contents;
        int stacksLineLength = chars.IndexOf((byte)'\n') + 1;
        const int charsPerStack = 4;
        int stackCount = stacksLineLength / charsPerStack;
        var stackLists = new List<byte>[stackCount];
        for (int j = 0; j < stackCount; j++)
        {
            stackLists[j] = new();
        }

        int i = 1;
        do
        {
            for (int s = 0; s < stackCount; s++)
            {
                byte crate = chars[i];
                if (crate != ' ')
                {
                    stackLists[s].Add(crate);
                }
                i += charsPerStack;
            }
        } while (chars[i] != '1');

        var instructions = new List<Instruction>();
        const string moveTxt = "move ";
        const string fromTxt = " from ";
        const string toTxt = " to ";

        i += stacksLineLength + moveTxt.Length;
        do
        {
            int count = chars[i] - '0';
            int next = chars[++i];
            if (next != ' ')
            { 
                count = 10*count + next - '0';
                ++i;
            }
            i += fromTxt.Length;

            int from = chars[i] - '0' - 1;
            i += toTxt.Length + 1;

            int to = chars[i] - '0' - 1;
            i += 2 + moveTxt.Length;
            instructions.Add(new((byte)count, (byte)from, (byte)to));
        } while (i < chars.Length);

        Instructions = instructions.ToArray();
        for (int s = 0; s < stackCount; s++)
        {
            stackLists[s].Reverse();
        }
        StackLists = stackLists;
    }

    [Benchmark]
    public override string Solve1()
    {
        var stackLists = StackLists;
        var stacks = new Stack<byte>[stackLists.Length];
        for (int i = 0; i < stackLists.Length; i++)
        {
            stacks[i] = new(stackLists[i]);
        }

        foreach (var instr in Instructions)
        {
            var from = stacks[instr.From];
            var to = stacks[instr.To];
            to.EnsureCapacity(to.Count + instr.Count);
            for (int i = 0; i < instr.Count; i++)
            {
                to.Push(from.Pop());
            }
        }

        return string.Create(stackLists.Length, stacks, static (Span<char> str, Stack<byte>[] stacks) =>
        {
            for (int i = 0; i < stacks.Length; i++)
            {
                str[i] = (char)stacks[i].Peek();
            }
        });
    }

    [Benchmark]
    public override string Solve2()
    {
        var stackLists = StackLists;
        var stacks = new List<byte>[stackLists.Length];
        for (int i = 0; i < stackLists.Length; i++)
        {
            stacks[i] = new(stackLists[i]);
        }

        foreach (var instr in Instructions)
        {
            List<byte> from = stacks[instr.From];
            List<byte> to = stacks[instr.To];
            to.AddRange(from.GetRange(from.Count - instr.Count, instr.Count));
            from.RemoveRange(from.Count - instr.Count, instr.Count);
        }

        return string.Create(stackLists.Length, stacks, static (Span<char> str, List<byte>[] stacks) =>
        {
            for (int i = 0; i < stacks.Length; i++)
            {
                str[i] = (char)stacks[i][^1];
            }
        });
    }
}
