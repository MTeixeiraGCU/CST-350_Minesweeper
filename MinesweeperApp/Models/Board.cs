using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    public class Board
    {
        public int Size { get; set; }
        public Cell[,] Grid { get; set; }
        public int Difficulty { get; set; }

        public float[] difficultySettings = { 0.12f, 0.14f, 0.16f };

        public Board(int difficulty)
        {
            Difficulty = difficulty;
            Size = difficulty * 10;
            Grid = new Cell[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    //Id for each cell will be (row * width + col)
                    Grid[i, j] = new Cell((i * Size) + j);
                }
            }
        }

        //This method sets up the game board with mines based on the current difficulty
        public void SetupLiveNeighbors()
        {
            //check for invalid difficulties and default to medium
            if (Difficulty < 1 || Difficulty > 3)
            {
                Difficulty = 1;
            }

            //random number generator
            var rand = new Random();

            //first determine the number of mines based on difficulty
            var numberOfMines = (int)(Size * Size) * difficultySettings[Difficulty - 1];

            for (int i = 0; i < numberOfMines; i++)
            {
                //randomly place mine
                var row = rand.Next(0, Size);
                var col = rand.Next(0, Size);

                //check for previously placed mine
                if (Grid[row, col].Mine)
                {
                    //go back and try to place it again
                    i--;
                }
                else
                {
                    Grid[row, col].Mine = true;
                }
            }
        }

        //This method goes through each cell and calculates how many mines it is touching
        public void CalculateLiveNeighbors()
        {
            //loop through each Cell to populate its neighbors
            foreach (var cell in Grid)
            {
                //loop through each cell surrounding the current cell in a 3x3 Grid
                for (int m = -1; m < 2; m++)
                {
                    for (int n = -1; n < 2; n++)
                    {
                        //grab the current neighbor's position
                        var row = (cell.Id / Size) + m;
                        var col = (cell.Id % Size) + n;

                        //check if we are off the board
                        if (!withinBounds(row, col))
                        {
                            continue;
                        }

                        //check for mine
                        if (Grid[row, col].Mine)
                        {
                            cell.LiveNeighbors++;
                        }
                    }
                }
            }
        }

        //this method checks that we are inside the bounds
        private bool withinBounds(int row, int col)
        {
            return row >= 0 && col >= 0 && row < Size && col < Size;
        }

        // This method checks the entire board for all visits to determine endgame status
        public bool CheckBoardVisits()
        {
            foreach (var cell in Grid)
            {
                //if the cell has not been visited and we are not a mine we are not finished
                if (!cell.Visited && !cell.Mine)
                {
                    return false;
                }
            }
            return true;
        }

        //This method visits a cell on the grid. It will return true if the cell was a mine otherwise false.
        public bool MakeMove(int row, int col)
        {
            if (!withinBounds(row, col))
            {
                return false;
            }

            //visit the cell
            Grid[row, col].Visited = true;

            //check for no neighbors
            if (Grid[row, col].LiveNeighbors == 0)
            {
                //recursively fill out all 0 neighbors cells
                floodFill(row, col);
            }

            return Grid[row, col].Mine;
        }

        //Recursive function to find all empty neighbors and set them to visited
        private void floodFill(int row, int col)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //check within bounds
                    if (!withinBounds(row + i, col + j))
                    {
                        continue;
                    }

                    //check if we have been to this cell already
                    if (Grid[row + i, col + j].Visited)
                    {
                        continue;
                    }

                    //visit the cell and check for continued recursion
                    Grid[row + i, col + j].Visited = true;

                    if (Grid[row + i, col + j].LiveNeighbors == 0)
                    {
                        floodFill(row + i, col + j);
                    }
                }
            }
        }
    }
}
