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
            int searchRows = Rows - 2;
            //int numVisible = 2 * (Columns + searchRows);
            int searchColumns = Columns - 2;
            Span<bool> knownVisible = stackalloc bool[Columns * Rows];
            knownVisible.Clear();


            //Span<(int min, int max)> horizontalRanges = stackalloc (int min, int max)[searchRows];
            //Span<(int min, int max)> verticalRanges = stackalloc (int min, int max)[searchColumns];

            //Span<byte> columnPrevMaxHeights = stackalloc byte[searchColumns];
            //for (int col = 0; col < Columns; col++)
            //{
            //    knownVisible[col] = true;
            //    knownVisible[(Rows - 1) * Columns + col] = true;
            //    if (col >= 1 && col < Columns - 1)
            //    {
            //        columnPrevMaxHeights[col - 1] = Grid[col];
            //    }
            //}

            //for (int row = 1; row < Rows - 1; row++)
            //{
            //    knownVisible[row * Columns] = true;
            //    knownVisible[(row + 1) * Columns - 1] = true;
            //}


            //for (int row = 0; row < searchRows; row++)
            //{
            //    int rowIndex = (row + 1) * Columns;
            //    var prevMaxHeight = Grid[rowIndex];
            //    for (int col = 0; col < searchColumns; col++)
            //    {
            //        int i = rowIndex + col + 1;
            //        var height = Grid[i];
            //        bool visible = false;
            //        if (height > prevMaxHeight)
            //        {
            //            prevMaxHeight = height;
            //            visible = true;
            //        }

            //        if (height > columnPrevMaxHeights[col])
            //        {
            //            columnPrevMaxHeights[col] = height;
            //            visible = true;
            //        }

            //        if (visible)
            //        {
            //            knownVisible[i] = true;
            //            continue;
            //        }

            //        visible = true;
            //        for (int j = col + 1; j < searchColumns; j++)
            //        {
            //            if (height <= Grid[rowIndex + j + 1])
            //            {
            //                visible = false;
            //                break;
            //            }
            //        }

            //        if (visible)
            //        {
            //            knownVisible[i] = true;
            //            continue;
            //        }

            //        visible = true;
            //        for (int j = row + 1; j < searchRows; j++)
            //        {
            //            if (height <= Grid[(j + 1) * Columns + col])
            //            {
            //                visible = false;
            //                break;
            //            }
            //        }

            //        if (visible)
            //        {
            //            knownVisible[i] = true;
            //        }
            //    }
            //}


            //// Process all rows
            //for (int row = 0; row < searchRows; row++)
            //{
            //    int startIndex = (row + 1) * Columns;
            //    var maxHeight = Grid[startIndex];
            //    int maxCol = 0;
            //    int col = 0;
            //    for (; col < searchColumns; col++)
            //    {
            //        int i = startIndex + col + 1;
            //        var height = Grid[i];
            //        if (height > maxHeight)
            //        {
            //            maxHeight = height;
            //            maxCol = col;
            //            if (!knownVisible[i])
            //            {
            //                //++numVisible;
            //                knownVisible[i] = true;
            //            }
            //        }
            //    }

            //    var minCol = searchColumns - 1;
            //    maxHeight = Grid[startIndex + Columns - 1];
            //    for (col = searchColumns - 1; col > maxCol; col--)
            //    {
            //        int i = startIndex + col + 1;
            //        var height = Grid[i];
            //        if (height > maxHeight)
            //        {
            //            minCol = col;
            //            maxHeight = height;
            //            if (!knownVisible[i])
            //            {
            //                //++numVisible;
            //                knownVisible[i] = true;
            //            }
            //        }
            //    }

            //    horizontalRanges[row] = (maxCol, minCol);
            //}

            //// Process all columns
            //for (int col = 0; col < searchColumns; col++)
            //{
            //    var maxHeight = Grid[col + 1];
            //    int maxRow = 0;
            //    int row = 0;
            //    for (; row < searchRows; row++)
            //    {
            //        int i = (row + 1) * Columns + col + 1;
            //        var height = Grid[i];
            //        if (height > maxHeight)
            //        {
            //            maxHeight = height;
            //            maxRow = row;
            //            if (!knownVisible[i])
            //            {
            //                //++numVisible;
            //                knownVisible[i] = true;
            //            }
            //        }
            //    }

            //    var minRow = searchRows - 1;
            //    maxHeight = Grid[(Rows - 1) * Columns + col];
            //    for (row = searchRows - 1; row > maxRow; row--)
            //    {
            //        int i = (row + 1) * Columns + col + 1;
            //        var height = Grid[i];
            //        if (height > maxHeight)
            //        {
            //            minRow = row;
            //            maxHeight = height;
            //            if (!knownVisible[i])
            //            {
            //                //++numVisible;
            //                knownVisible[i] = true;
            //            }
            //        }
            //    }

            //    verticalRanges[col] = (maxRow, minRow);
            //}

            int numVisible = 0;
            for (int i = 0; i < knownVisible.Length; i++)
            {
                if (knownVisible[i])
                {
                    ++numVisible;
                }
            }

            //using (var file = new StreamWriter("D:/test.txt"))
            //{
            //    int i = 0;
            //    for (int row = 0; row < Rows; row++)
            //    {
            //        for (int col = 0; col < Columns; col++)
            //        {
            //            if (col >= 1 && col < Columns - 1)
            //            {
            //                var vertRange = verticalRanges[col - 1];
            //                file.Write(row - 1 <= vertRange.min + 1 || row - 1 >= vertRange.max + 1 ? '│' : ' ');
            //            }
            //            else
            //            {
            //                file.Write('|');
            //            }
            //            file.Write((char)(Grid[i] + '0'));
            //            file.Write(knownVisible[i] ? 'x' : 'o');
            //            file.Write(' ');
            //            ++i;
            //        }

            //        file.WriteLine();

            //        if (row >= 1 && row < Rows - 1)
            //        {
            //            var horizRange = horizontalRanges[row - 1];
            //            file.Write("├───");
            //            for (int j = 0; j <= horizRange.min; j++)
            //            {
            //                var vertRange = verticalRanges[j];
            //                file.Write(row - 1 <= vertRange.min + 1 || row - 1 >= vertRange.max + 1 ? '┼' : '─');
            //                file.Write("───");
            //            }
            //            file.Write(new string(' ', (horizRange.max - horizRange.min - 1) * 4));
            //            if (horizRange.max < Columns - 1)
            //            {
            //                var vertRange = verticalRanges[horizRange.max];
            //                file.Write(row <= vertRange.min + 1 || row >= vertRange.max + 1 ? '├' : '─');
            //                file.Write("───");
            //                for (int j = horizRange.max + 1; j < Columns - 1; j++)
            //                {
            //                    vertRange = verticalRanges[j - 1];
            //                    file.Write(row - 1 <= vertRange.min + 1 || row - 1 >= vertRange.max + 1 ? '┼' : '─');
            //                    file.Write("───");
            //                }
            //            }
            //            file.WriteLine();
            //        }
            //        else
            //        {
            //            file.WriteLine(new string('─', Columns * 4));
            //        }
            //    }
            //}
            return numVisible.ToString();
        }

        public override string Solve2()
        {
            throw new NotImplementedException();
        }
    }
}
