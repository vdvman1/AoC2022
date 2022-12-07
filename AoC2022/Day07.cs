using System.Text;

namespace AoC2022;

public partial class Day07 : DayBase
{
    private class Directory
    {
        public readonly Dictionary<string, Directory> Directories = new();
        public int Size = 0;
    }

    private Directory RootDir = null!;

    [Benchmark]
    public override void ParseData()
    {
        ReadOnlySpan<byte> chars = Contents;
        var root = new Directory();
        var path = new Stack<Directory>();
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
                        if (!currentDir.Directories!.TryGetValue(name, out Directory? child))
                        {
                            child = new();
                            currentDir.Directories.Add(name, child);
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
                        if (!currentDir.Directories!.ContainsKey(name))
                        {
                            currentDir.Directories.Add(name, new());
                        }
                        i += charsToNewline + 1;
                    }
                    else
                    {
                        int size = chars[i] - '0';
                        ++i;
                        while (chars[i] != ' ')
                        {
                            size = 10 * size + chars[i] - '0';
                            ++i;
                        }

                        ++i;
                        var charsToNewline = chars[i..].IndexOf((byte)'\n');
                        currentDir.Size += size;
                        foreach (Directory dir in path)
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

        void ProcessDir(Directory dir)
        {
            if (dir.Size <= 100000)
            {
                total += dir.Size;
            }

            foreach (Directory child in dir.Directories!.Values)
            {
                ProcessDir(child);
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

        void ProcessDir(Directory dir)
        {
            foreach (Directory child in dir.Directories!.Values)
            {
                ProcessDir(child);
            }

            if (dir.Size >= spaceNeeded && dir.Size <= minSpaceFreed)
            {
                minSpaceFreed = dir.Size;
            }
        }

        foreach (Directory child in RootDir.Directories!.Values)
        {
            ProcessDir(child);
        }

        return minSpaceFreed.ToString();
    }
}
