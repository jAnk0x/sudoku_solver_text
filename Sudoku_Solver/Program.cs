using System;
using System.Collections.Generic;

namespace Sudoku_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            /*int[,] grid = { { 0, 0, 2, 9, 8, 0, 5, 0, 0},
                            { 4, 0, 0, 0, 7, 0, 0, 1, 3},
                            { 0, 3, 9, 6, 0, 4, 0, 7, 0},
                            { 2, 0, 0, 0, 5, 6, 4, 0, 0},
                            { 8, 4, 0, 3, 0, 0, 2, 0, 1},
                            { 9, 0, 7, 0, 0, 1, 0, 8, 6},
                            { 6, 0, 0, 7, 0, 5, 1, 3, 0},
                            { 0, 9, 1, 4, 0, 0, 0, 0, 5},
                            { 0, 2, 0, 0, 3, 0, 6, 0, 8}};

            int[,] grid = { { 0, 0, 8, 0, 0, 0, 0, 0, 0},
                            { 0, 3, 0, 0, 5, 0, 0, 0, 0},
                            { 0, 0, 0, 3, 0, 7, 0, 0, 0},
                            { 0, 0, 1, 0, 0, 0, 5, 0, 8},
                            { 5, 0, 7, 9, 0, 8, 6, 0, 0},
                            { 0, 9, 0, 0, 0, 0, 0, 0, 0},
                            { 0, 1, 0, 7, 0, 0, 0, 0, 4},
                            { 0, 4, 0, 0, 1, 0, 8, 0, 6},
                            { 0, 0, 0, 0, 0, 5, 0, 2, 0}};

            int[,] grid = { { 0, 2, 0, 6, 3, 0, 0, 0, 8},
                            { 0, 0, 0, 0, 0, 0, 7, 0, 0},
                            { 8, 0, 1, 0, 0, 0, 0, 0, 0},
                            { 1, 5, 0, 0, 0, 0, 0, 0, 0},
                            { 0, 0, 0, 8, 0, 6, 0, 0, 0},
                            { 0, 0, 3, 4, 5, 0, 0, 0, 0},
                            { 0, 0, 0, 0, 1, 0, 5, 3, 0},
                            { 0, 0, 0, 2, 0, 5, 0, 1, 6},
                            { 7, 0, 0, 0, 0, 0, 0, 0, 4}};
                            */
            
            int[][] grid = new int[9][];
            Console.WriteLine("Press '1' to generate grid");
            Console.WriteLine("Press '2' to make your own grid");
            Console.Write("Input: ");
            var input = Console.ReadKey();
            int option = 0;
            Console.WriteLine();
            Console.WriteLine();

            if (char.IsDigit(input.KeyChar))
                option = int.Parse(input.KeyChar.ToString());

            if (option == 1)
                grid = GenerateGrid();
            else if (option == 2)
            {
                Console.WriteLine("Enter your grid one line at a time: ");
                for(int i = 0; i < 9; i++)
                {
                    char[] charLine = Console.ReadLine().ToCharArray();
                    int[] intLine = new int[9];
                    for (int z = 0; z < 9; z++)
                    {
                        int num = (int) char.GetNumericValue(charLine[z]);
                        if (num != -1)
                            intLine[z] = num;
                        else
                        {
                            Console.WriteLine("Wrong number");
                            break;
                        }
                    }
                    grid[i] = intLine;
                }
                Console.WriteLine();
            }
            PrintGrid(grid);
            Console.WriteLine();

            Console.WriteLine("Would you like to solve it? (y/n)");
            input = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();

            if (input.KeyChar == 'y')
                PrintGrid(SolveGrid(grid));
        }

        private static void PrintGrid(int[][] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int z = 0; z < 9; z++)
                {
                    Console.Write(grid[i][z] + " ");
                    if (z == 2 || z == 5)
                        Console.Write(" ");
                }
                Console.WriteLine();
                if (i == 2 || i == 5)
                    Console.WriteLine();
            }
        }

        private static int[][] SolveGrid(int[][] grid)
        {
            Coordinates coordinates = FirstZeroCoordinates(grid);
            if (!SolveGridUtil(grid, coordinates))
                Console.WriteLine("Unable to solve");
            return grid;
        }

        private static bool IsTheOnlyValue(int[][] grid, int val, Coordinates valCoords)
        {
            for(int i = 0; i < 9; i++)
            {
                if (i == val)
                    continue;
                if (TryValue(grid, valCoords, i))
                    return false;
            }
            return true;
        }

        private static bool SolveGridUtil(int[][] grid, Coordinates coordinates)
        {
            if (coordinates.X == -1)
                return true;

            for (int i = 1; i < 10; i++)
            {
                //PrintGrid(grid);
                //Console.WriteLine();
                if (TryValue(grid, coordinates, i))
                {
                    grid[coordinates.Y][coordinates.X] = i;
                    if (SolveGridUtil(grid, FirstZeroCoordinates(grid)))
                        return true;
                    else
                        grid[coordinates.Y][coordinates.X] = 0;
                }
            }
            return false;
        }

        private static bool TryValue(int[][] grid, Coordinates coordinates, int value)
        {
            for (int i = 0; i < 9; i++)
                if (grid[i][coordinates.X] == value || grid[coordinates.Y][i] == value)
                    return false;
            return CheckDublicateSubGrid(grid, coordinates, value);
        }

        private static bool CheckDublicateSubGrid(int[][] grid, Coordinates coordinates, int value)
        {
            int xStart, yStart;

            if (coordinates.X / 3 == 0)
                xStart = 0;
            else if (coordinates.X / 3 == 1)
                xStart = 3;
            else
                xStart = 6;
            if (coordinates.Y / 3 == 0)
                yStart = 0;
            else if (coordinates.Y / 3 == 1)
                yStart = 3;
            else
                yStart = 6;

            for (int i = 0; i < 3; i++)
            {
                for (int z = 0; z < 3; z++)
                {
                    if (grid[yStart + i][xStart + z] == value)
                        return false;
                }
            }
            return true;
        }

        private static Coordinates FirstZeroCoordinates(int[][] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int z = 0; z < 9; z++)
                    if (grid[i][z] == 0)
                        return new Coordinates { Y = i, X = z };
            }
            return new Coordinates();
        }

        private static int[][] GenerateGrid()
        {
            List<Coordinates> triedToRemove = new List<Coordinates>();
            int[][] full = GenerateFullGrid();
            while(triedToRemove.Count < 81)
                RemoveVal(full, triedToRemove);
            return full;
        }

        private static void RemoveVal(int[][] full, List<Coordinates> triedToRemove)
        {
            if (triedToRemove.Count >= 81)
                return;

            var rand = new Random();
            Coordinates coords = new Coordinates { X = rand.Next(0, 9), Y = rand.Next(0, 9) };
            int old = full[coords.Y][coords.X];
            if (old == 0 || ContainsCoordinates(triedToRemove, coords))
                RemoveVal(full, triedToRemove);

            full[coords.Y][coords.X] = 0;
            triedToRemove.Add(coords);
            if (!IsTheOnlyValue(full, old, coords))
            {
                full[coords.Y][coords.X] = old;
                RemoveVal(full, triedToRemove);
            }
        }

        private static int[][] GenerateFullGrid()
        {
            int[][] grid = new int[9][];
            int[] nums = new int[9];
            var rand = new Random();
            int i = 0;

            while (nums[8] == 0)
            {
                int r = rand.Next(1, 10);
                if (!Array.Exists(nums, x => x == r))
                {
                    nums[i] = r;
                    i++;
                }
            }
            int[] sh1 = ShiftArray(nums, 1);
            int[] sh3 = ShiftArray(nums, 3);
            grid[0] = nums;
            grid[1] = ShiftArray(grid[0], 3);
            grid[2] = ShiftArray(grid[1], 3);
            grid[3] = ShiftArray(grid[2], 1);
            grid[4] = ShiftArray(grid[3], 3);
            grid[5] = ShiftArray(grid[4], 3);
            grid[6] = ShiftArray(grid[5], 1);
            grid[7] = ShiftArray(grid[6], 3);
            grid[8] = ShiftArray(grid[7], 3);

            return grid;
        }

        private static int[] ShiftArray(int[] arr, int num)
        {
            int[] sArr = new int[9];
            int c = 0;
            for (int i = 0; i < 9; i++)
            {
                if (i + num >= 9)
                {
                    sArr[i] = arr[c];
                    c++;
                }
                else
                    sArr[i] = arr[i + num];
            }
            return sArr;
        }
    
        private static bool ContainsCoordinates(List<Coordinates> list, Coordinates coords)
        {
            foreach(Coordinates c in list)
            {
                if (c.X == coords.X && c.Y == coords.Y)
                    return true;
            }
            return false;
        }
    }
}
