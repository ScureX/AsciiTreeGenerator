using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AsciiTreeGenerator
{
    internal class Program
    {
        static Random random = new Random();

        const int air = 0;
        const int stem = 1;
        const int leaf = 2;

        static int stemProbability = 62;
        static int leafProbability = 65;
        static int variation = 10;

        static List<string> stems = new List<string>() { "|", "/", "\\" };
        static List<string> leaves = new List<string>() { "o", "O", "0" };

        static void Main(string[] args)
        {
            int size = 50;
            int amount = 1;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--size" when i + 1 < args.Length:
                    case "-s" when i + 1 < args.Length:
                        int.TryParse(args[i + 1], out size);
                        break;
                    case "--amount" when i + 1 < args.Length:
                    case "-a" when i + 1 < args.Length:
                        int.TryParse(args[i + 1], out amount);
                        break;
                    case "--variation" when i + 1 < args.Length:
                    case "-v" when i + 1 < args.Length:
                        int.TryParse(args[i + 1], out variation);
                        break;
                }
            }

            for (int i = 0; i < amount; i++)
            {
                MakeTree(random, size);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
            //Console.ReadLine();
        }

        static int[] GenerateFirstRow(int size)
        {
            int[] row = new int[size]; // array is initialized with zeros

            // replace the zero in the middle with stem
            for (int i = 0; i < size; i++){
                if (i == size / 2)
                {
                    row[i] = stem;
                    break;
                }
            }

            return row;
        }

        static void MakeTree(Random random, int size)
        {
            
            int[][] tree = new int[size][];
            tree[0] = GenerateFirstRow(size);

            // for each row
            for (int row = 1; row < tree[0].Length; row++)
            {
                int[] newRow = new int[tree[0].Length];

                int stemProbabilityWithVar = stemProbability + random.Next(-variation - row, variation - row);
                int leafProbabilityWithVar = leafProbability + random.Next(-variation - row, variation - row);

                // for each column in row
                for (int column = 0; column < tree[0].Length; column++)
                {
                    // if int adjacent in column above is stem make this row/column stem or leaf
                    if (IsAdjacent(tree[row - 1], column, stem))
                    {
                        int rand = random.Next(1, 101);
                        if (rand > (100 - stemProbabilityWithVar))
                        {
                            newRow[column] = stem;
                        }
                        else if (rand > (100 - leafProbabilityWithVar))
                        {
                            newRow[column] = leaf;
                        }
                        else
                        {
                            newRow[column] = air;
                        }
                    }
                    // if adjacent is leaf, give change to become leaf
                    else if (IsAdjacent(tree[row - 1], column, leaf, true))
                    {
                        if (random.Next(1, 101) > (100 - leafProbabilityWithVar))
                        {
                            newRow[column] = leaf;
                        }
                    }
                    // not connected, make air
                    else
                    {
                        newRow[column] = air;
                    }
                }

                tree[row] = newRow;
            }
            PrintReverse(tree);
        }

        static bool IsAdjacent(int[] rowBefore, int index, int type, bool extended = false)
        {
            if (rowBefore[index] == type)
                return true;

            if (index - 1 > 0)
                if (rowBefore[index - 1] == type)
                    return true;

            if (index + 1 < rowBefore.Length)
                if (rowBefore[index + 1] == type)
                    return true;

            if (extended && index - 2 > 0)
                if (rowBefore[index - 2] == type)
                    return true;

            if (extended && index + 2 < rowBefore.Length)
                if (rowBefore[index + 2] == type)
                    return true;

            return false;
        }

        static void PrintReverse(int[][] tree)
        {
            // for each column top to bottom
            for (int row = tree.Length - 1; row >= 0; row--)
            {
                // print each element at that row
                foreach (int val in tree[row])
                {
                    if(val == air)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    else if (val == stem)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(stems[random.Next(stems.Count)]);
                    }
                    else if (val == leaf)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(leaves[random.Next(leaves.Count)]);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
