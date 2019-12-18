using System;

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
                            */

            /*int[,] grid = { { 0, 0, 8, 0, 0, 0, 0, 0, 0},
                            { 0, 3, 0, 0, 5, 0, 0, 0, 0},
                            { 0, 0, 0, 3, 0, 7, 0, 0, 0},
                            { 0, 0, 1, 0, 0, 0, 5, 0, 8},
                            { 5, 0, 7, 9, 0, 8, 6, 0, 0},
                            { 0, 9, 0, 0, 0, 0, 0, 0, 0},
                            { 0, 1, 0, 7, 0, 0, 0, 0, 4},
                            { 0, 4, 0, 0, 1, 0, 8, 0, 6},
                            { 0, 0, 0, 0, 0, 5, 0, 2, 0}};
                            */

            int[,] grid = { { 0, 2, 0, 6, 3, 0, 0, 0, 8},
                            { 0, 0, 0, 0, 0, 0, 7, 0, 0},
                            { 8, 0, 1, 0, 0, 0, 0, 0, 0},
                            { 1, 5, 0, 0, 0, 0, 0, 0, 0},
                            { 0, 0, 0, 8, 0, 6, 0, 0, 0},
                            { 0, 0, 3, 4, 5, 0, 0, 0, 0},
                            { 0, 0, 0, 0, 1, 0, 5, 3, 0},
                            { 0, 0, 0, 2, 0, 5, 0, 1, 6},
                            { 7, 0, 0, 0, 0, 0, 0, 0, 4}};

            PrintGrid(SolveGrid(grid));
        }

        private static void PrintGrid(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int z = 0; z < 9; z++)
                    Console.Write(grid[i, z] + " ");
                Console.WriteLine();
            }
        }

        private static int[,] SolveGrid(int[,] grid)
        {
            Coordinates coordinates = FirstZeroCoordinates(grid);
            if (!SolveGridUtil(grid, coordinates))
                Console.WriteLine("Unable to solve");
            return grid;
        }

        private static bool SolveGridUtil(int[,] grid, Coordinates coordinates)
        {
            if (coordinates.X == -1)
                return true;

            for (int i = 1; i < 10; i++)
            {
                //PrintGrid(grid);
                //Console.WriteLine();
                if (TryValue(grid, coordinates, i))
                {
                    grid[coordinates.Y, coordinates.X] = i;
                    if (SolveGridUtil(grid, FirstZeroCoordinates(grid)))
                        return true;
                    else
                        grid[coordinates.Y, coordinates.X] = 0;
                }
            }
            return false;
        }

        private static bool TryValue(int[,] grid, Coordinates coordinates, int value)
        {
            for (int i = 0; i < 9; i++)
                if (grid[i, coordinates.X] == value || grid[coordinates.Y, i] == value)
                    return false;
            return CheckDublicateSubGrid(grid, coordinates, value);
        }

        private static bool CheckDublicateSubGrid(int[,] grid, Coordinates coordinates, int value)
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
                    if (grid[yStart + i, xStart + z] == value)
                        return false;
                }
            }
            return true;
        }

        private static Coordinates FirstZeroCoordinates(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int z = 0; z < 9; z++)
                    if (grid[i, z] == 0)
                        return new Coordinates { Y = i, X = z };
            }
            return new Coordinates();
        }
    }
}
