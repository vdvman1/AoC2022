using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    public partial class Day08 : DayBase
    {
        private byte[] Grid = null!;
        private int Columns = 0;
        private int Rows = 0;

        public override void ParseData()
        {
            ReadOnlySpan<byte> chars = Contents;
            var columns = chars.IndexOf((byte)'\n');
            var rows = chars.Length / (columns + 1);
            var grid = new byte[rows * columns];
            int i = 0;
            int j = 0;
            do
            {
                for (int col = 0; col < columns; col++)
                {
                    grid[j++] = (byte)(chars[i++] - '0');
                }
                ++i;
            } while (i < chars.Length);

            Grid = grid;
            Columns = columns;
            Rows = rows;
        }

        public override string Solve1()
        {
            int numVisible = 0;

            for (int row = 0; row < Rows; row++)
            {
                int rowStart = row * Columns;
                for (int col = 0; col < Columns; col++)
                {
                    if (row == 0 || row == Rows - 1 || col == 0 || col == Columns - 1)
                    {
                        ++numVisible;
                        continue;
                    }

                    int height = Grid[rowStart + col];
                    bool visible = true;
                    for (int i = 0; i < col; i++)
                    {
                        if (height <= Grid[rowStart + i])
                        {
                            visible = false;
                            break;
                        }
                    }

                    if (visible)
                    {
                        ++numVisible;
                        continue;
                    }

                    visible = true;
                    for (int i = col + 1; i < Columns; i++)
                    {
                        if (height <= Grid[rowStart + i])
                        {
                            visible = false;
                            break;
                        }
                    }

                    if (visible)
                    {
                        ++numVisible;
                        continue;
                    }

                    visible = true;
                    for (int i = 0; i < row; i++)
                    {
                        if (height <= Grid[i*Columns + col])
                        {
                            visible = false;
                            break;
                        }
                    }

                    if (visible)
                    {
                        ++numVisible;
                        continue;
                    }

                    visible = true;
                    for (int i = row + 1; i < Rows; i++)
                    {
                        if (height <= Grid[i * Columns + col])
                        {
                            visible = false;
                            break;
                        }
                    }

                    if (visible)
                    {
                        ++numVisible;
                    }
                }
            }

            return numVisible.ToString();
        }

        public override string Solve2()
        {
            throw new NotImplementedException();
        }
    }
}
