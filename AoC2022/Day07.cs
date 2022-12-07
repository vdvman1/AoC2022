namespace AoC2022;

public partial class Day07 : DayBase
{
    private class DirTrie
    {
        public Directory? Directory;
        public ReadOnlyMemory<byte> Prefix;
        public DirTrie?[]? Trie;
    }

    private class Directory
    {
        public DirTrie? Trie = null;
        public readonly List<Directory> ChildDirectories = new();
        public int Size = 0;

        public Directory VisitDir(ReadOnlySpan<byte> name)
        {
            if (Trie is null)
            {
                var dir = new Directory();
                Trie = new()
                {
                    Directory = dir,
                    Prefix = name.ToArray()
                };
                ChildDirectories.Add(dir);
                return dir;
            }

            ref DirTrie trieRef = ref Trie!;

            do
            {
                var trie = trieRef;
                var prefix = trie.Prefix.Span;
                int commonLen = 0;
                do
                {
                    if (commonLen == prefix.Length)
                    {
                        if (commonLen == name.Length) // prefix == name
                        {
                            return trie.Directory!;
                        }

                        // name.StartsWith(prefix) == true
                        var c = name[commonLen] - 'a';
                        name = name[(commonLen + 1)..];
                        if (trie.Trie is null)
                        {
                            trie.Trie = new DirTrie['z' - 'a' + 1];
                            var dir = new Directory();
                            trie.Trie[c] = new()
                            {
                                Directory = dir,
                                Prefix = name.Length == 0 ? default(ReadOnlyMemory<byte>) : name.ToArray()
                            };
                            ChildDirectories.Add(dir);
                            return dir;
                        }
                        else if (trie.Trie[c] is null)
                        {
                            var dir = new Directory();
                            trie.Trie[c] = new()
                            {
                                Directory = dir,
                                Prefix = name.Length == 0 ? default(ReadOnlyMemory<byte>) : name.ToArray()
                            };
                            ChildDirectories.Add(dir);
                            return dir;
                        }
                        else
                        {
                            trieRef = ref trie.Trie[c]!;
                            break;
                        }
                    }
                    else if (commonLen == name.Length) // prefix.StartsWith(name)
                    {
                        var dir = new Directory();
                        ChildDirectories.Add(dir);

                        var arr = new DirTrie['z' - 'a' + 1];
                        arr[prefix[name.Length] - 'a'] = trie;

                        trie.Prefix = trie.Prefix[(name.Length + 1)..];

                        trieRef = new()
                        {
                            Directory = dir,
                            Prefix = trie.Prefix[..name.Length],
                            Trie = arr
                        };

                        return dir;
                    }
                    else if (name[commonLen] != prefix[commonLen]) // Shared prefix
                    {
                        var commonPrefix = trie.Prefix[..commonLen];
                        trie.Prefix = trie.Prefix[(commonLen + 1)..];

                        var dir = new Directory();
                        ChildDirectories.Add(dir);

                        var arr = new DirTrie['z' - 'a' + 1];
                        arr[prefix[commonLen] - 'a'] = trie;
                        arr[name[commonLen] - 'a'] = new DirTrie()
                        {
                            Directory = dir,
                            Prefix = name.Length == commonLen + 1 ? default(ReadOnlyMemory<byte>) : name[(commonLen + 1)..].ToArray()
                        };

                        trieRef = new()
                        {
                            Prefix = commonPrefix,
                            Trie = arr
                        };

                        return dir;
                    }
                    else
                    {
                        ++commonLen;
                    }
                } while (true);
            } while (true);
        }
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
                        path.Push(currentDir);
                        currentDir = currentDir.VisitDir(chars.Slice(i, charsToNewline));
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
                        currentDir.VisitDir(chars.Slice(i, charsToNewline));
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

            for (int i = 0; i < dir.ChildDirectories.Count; i++)
            {
                ProcessDir(dir.ChildDirectories[i]);
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
            if (dir.Size <= minSpaceFreed)
            {
                minSpaceFreed = dir.Size;
            }

            for (int i = 0; i < dir.ChildDirectories.Count; i++)
            {
                Directory child = dir.ChildDirectories[i];
                if (child.Size >= spaceNeeded)
                {
                    ProcessDir(child);
                }
            }
        }

        for (int i = 0; i < RootDir.ChildDirectories.Count; i++)
        {
            Directory dir = RootDir.ChildDirectories[i];
            if (dir.Size >= spaceNeeded)
            {
                ProcessDir(dir);
            }
        }

        return minSpaceFreed.ToString();
    }
}
