using System.Text;

namespace AoC2022;

public partial class Day07 : DayBase
{
    private class FsEntry
    {
        public readonly Dictionary<string, FsEntry>? Children;
        public int Size;

        public FsEntry()
        {
            Children = new();
            Size = 0;
        }

        public FsEntry(int size)
        {
            Children = null;
            Size = size;
        }
    }

    private FsEntry RootDir = null!;

    [Benchmark]
    public override void ParseData()
    {
        ReadOnlySpan<byte> chars = Contents;
        var root = new FsEntry();
        var path = new Stack<FsEntry>();
        var currentDir = root;

        const string cmdPrefix = "$ ";
        int i = cmdPrefix.Length;
        do
        {
            if (chars[i] == 'c') // cd
            {
                const string cd = "cd ";
                i += cd.Length;
                switch (chars[i])
                {
                    case (byte)'/':
                        currentDir = root;
                        path.Clear();

                        const string navRoot = "/\n";
                        i += navRoot.Length;
                        break;
                    case (byte)'.':
                        currentDir = path.Pop();

                        const string navUp = "..\n";
                        i += navUp.Length;
                        break;
                    default:
                        var charsToNewline = chars[i..].IndexOf((byte)'\n');
                        var name = Encoding.ASCII.GetString(chars.Slice(i, charsToNewline));
                        if (!currentDir.Children!.TryGetValue(name, out FsEntry? child))
                        {
                            child = new();
                            currentDir.Children.Add(name, child);
                        }

                        path.Push(currentDir);
                        currentDir = child;
                        i += charsToNewline + 1;
                        break;
                }
            }
            else // ls
            {
                const string ls = "ls\n";
                i += ls.Length;

                while (i < chars.Length && chars[i] != '$')
                {
                    if (chars[i] == 'd') // dir
                    {
                        const string dir = "dir ";
                        i += dir.Length;

                        var charsToNewline = chars[i..].IndexOf((byte)'\n');
                        var name = Encoding.ASCII.GetString(chars.Slice(i, charsToNewline));
                        if (!currentDir.Children!.ContainsKey(name))
                        {
                            currentDir.Children.Add(name, new());
                        }
                        i += charsToNewline + 1;
                    }
                    else
                    {
                        int size = chars[i] - '0';
                        ++i;
                        while (chars[i] != ' ')
                        {
                            size = 10*size + chars[i] - '0';
                            ++i;
                        }

                        ++i;
                        var charsToNewline = chars[i..].IndexOf((byte)'\n');
                        var name = Encoding.ASCII.GetString(chars.Slice(i, charsToNewline));
                        currentDir.Children!.Add(name, new(size));
                        currentDir.Size += size;
                        foreach (FsEntry dir in path)
                        {
                            dir.Size += size;
                        }
                        i += charsToNewline + 1;
                    }
                }
            }

            i += cmdPrefix.Length;
        } while (i < chars.Length);

        RootDir = root;
    }

    [Benchmark]
    public override string Solve1()
    {
        int total = 0;

        void ProcessDir(FsEntry dir)
        {
            if (dir.Size <= 100000)
            {
                total += dir.Size;
            }

            foreach (FsEntry child in dir.Children!.Values)
            {
                if (child.Children is not null)
                {
                    ProcessDir(child);
                }
            }
        }
        ProcessDir(RootDir);

        return total.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        const int DiskSize = 70000000;
        const int DesiredSpace = 30000000;

        int spaceAvailable = DiskSize - RootDir.Size;
        int spaceNeeded = DesiredSpace - spaceAvailable;
        int minSpaceFreed = RootDir.Size;

        void ProcessDir(FsEntry dir)
        {
            foreach (FsEntry child in dir.Children!.Values)
            {
                if (child.Children is not null)
                {
                    ProcessDir(child);
                }
            }

            if (dir.Size >= spaceNeeded && dir.Size <= minSpaceFreed)
            {
                minSpaceFreed = dir.Size;
            }
        }

        foreach (FsEntry child in RootDir.Children!.Values)
        {
            if (child.Children is not null)
            {
                ProcessDir(child);
            }
        }

        return minSpaceFreed.ToString();
    }
}
