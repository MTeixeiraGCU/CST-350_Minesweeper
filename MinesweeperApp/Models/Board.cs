using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    public class Board
    {
        public int Size { get; set; }
        public int Difficulty { get; set; }

        public const int GRID_SIZE = 10;

        public List<Cell> Grid 
        {
            get
            {
                List<Cell> cells = new List<Cell>();
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        cells.Add(grid[i, j]);
                    }
                }
                return cells;
            } 
        }

        private Cell[,] grid;

        private float[] difficultySettings = { 0.12f, 0.14f, 0.16f };

        public Board()
        {
        }

        public void CreateNewBoard(string difficulty)
        {
            Difficulty = getDifficultyFromString(difficulty);
            Size = GRID_SIZE;
            grid = new Cell[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    //Id for each cell will be (row * width + col)
                    grid[i, j] = new Cell((i * Size) + j);
                }
            }

            //create the mines on the board
            setupLiveNeighbors();

            //determine all the neighbor counts on the new board
            calculateLiveNeighbors();
        }

        //This method sets up the game board with mines based on the current difficulty
        private void setupLiveNeighbors()
        {
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
                if (grid[row, col].Mine)
                {
                    //go back and try to place it again
                    i--;
                }
                else
                {
                    grid[row, col].Mine = true;
                }
            }
        }

        //This method goes through each cell and calculates how many mines it is touching
        private void calculateLiveNeighbors()
        {
            //loop through each Cell to populate its neighbors
            foreach (var cell in grid)
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
                        if (grid[row, col].Mine)
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
            foreach (var cell in grid)
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
        public bool MakeMove(int id)
        {
            //grab the current cell's position
            var row = id / Size;
            var col = id % Size;

            if (!withinBounds(row, col))
            {
                return false;
            }

            //visit the cell
            grid[row, col].Visited = true;

            //check for no neighbors
            if (grid[row, col].LiveNeighbors == 0)
            {
                //recursively fill out all 0 neighbors cells
                floodFill(row, col);
            }

            return grid[row, col].Mine;
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
                    if (grid[row + i, col + j].Visited)
                    {
                        continue;
                    }

                    //visit the cell and check for continued recursion
                    grid[row + i, col + j].Visited = true;

                    if (grid[row + i, col + j].LiveNeighbors == 0)
                    {
                        floodFill(row + i, col + j);
                    }
                }
            }
        }

        //Sets the difficulty based on the string name of each level
        private int getDifficultyFromString(string difficutly)
        {
            //default level is medium
            int difficutlyLevel = 2;

            if (difficutly == "Easy")
            {
                difficutlyLevel = 1;
            }
            else if (difficutly == "Medium")
            {
                difficutlyLevel = 2;
            }
            else if (difficutly == "Hard")
            {
                difficutlyLevel = 3;
            }
            return difficutlyLevel;
        }
    }
}
