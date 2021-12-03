using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.BusinessServices
{
    public class GameboardBusinessService
    {
        public int Size {
            get {
                return gameBoard.Size;
            }
        }


        private const int GRID_SIZE = 10;
        private float[] DifficultySettings = { 0.12f, 0.14f, 0.16f };

        public List<Cell> Grid
        {
            get
            {
                List<Cell> cells = new List<Cell>();
                for (int i = 0; i < gameBoard.Size; i++)
                {
                    for (int j = 0; j < gameBoard.Size; j++)
                    {
                        cells.Add(gameBoard.Grid[i, j]);
                    }
                }
                return cells;
            }
        }

        private Board gameBoard;

        public GameboardBusinessService()
        {
            gameBoard = new Board();
        }

        //This method initiates a new game and randomizes the mines on the grid based off of the difficulty
        public void NewGame(string difficulty)
        {
            //create a new board
            gameBoard.Difficulty = getDifficultyFromString(difficulty);
            gameBoard.Size = GRID_SIZE;
            
            //clears the existing grid to a certain size
            clearGrid(gameBoard.Size);

            //create the mines on the board
            setupLiveNeighbors();

            //determine all the neighbor counts on the new board
            calculateLiveNeighbors();
        }

        //This method toggles the flagged status of a given cell
        public void ToggleFlag(int id)
        {
            //grab the current cell's position
            var row = id / gameBoard.Size;
            var col = id % gameBoard.Size;

            //check bounds first
            if (withinBounds(row, col))
            {
                //toggle flag
                gameBoard.Grid[row, col].Flagged = !gameBoard.Grid[row, col].Flagged;
            }
        }

        // This method checks the entire board for all visits to determine endgame status
        public bool CheckBoardVisits()
        {
            foreach (var cell in gameBoard.Grid)
            {
                //if the cell has not been visited and we are not a mine we are not finished
                if (!cell.Visited && !cell.Mine)
                {
                    return false;
                }
            }
            return true;
        }

        //This method clears the given board grid andsizes the new one based on the inputted size
        private void clearGrid(int size)
        {
            gameBoard.Grid = new Cell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //Id for each cell will be (row * width + col)
                    gameBoard.Grid[i, j] = new Cell((i * size) + j);
                }
            }
        }

        //This method sets up the game board with mines based on the current difficulty
        private void setupLiveNeighbors()
        {
            //random number generator
            var rand = new Random();

            //first determine the number of mines based on difficulty
            gameBoard.NumberOfMines = (int)((gameBoard.Size * gameBoard.Size) * DifficultySettings[gameBoard.Difficulty - 1]);

            for (int i = 0; i < gameBoard.NumberOfMines; i++)
            {
                //randomly place mine
                var row = rand.Next(0, gameBoard.Size);
                var col = rand.Next(0, gameBoard.Size);

                //check for previously placed mine
                if (gameBoard.Grid[row, col].Mine)
                {
                    //go back and try to place it again
                    i--;
                }
                else
                {
                    gameBoard.Grid[row, col].Mine = true;
                }
            }
        }

        //This method goes through each cell and calculates how many mines it is touching
        private void calculateLiveNeighbors()
        {
            //loop through each Cell to populate its neighbors
            foreach (var cell in gameBoard.Grid)
            {
                //loop through each cell surrounding the current cell in a 3x3 Grid
                for (int m = -1; m < 2; m++)
                {
                    for (int n = -1; n < 2; n++)
                    {
                        //grab the current neighbor's position
                        var row = (cell.Id / gameBoard.Size) + m;
                        var col = (cell.Id % gameBoard.Size) + n;

                        //check if we are off the board
                        if (!withinBounds(row, col))
                        {
                            continue;
                        }

                        //check for mine
                        if (gameBoard.Grid[row, col].Mine)
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
            return row >= 0 && col >= 0 && row < gameBoard.Size && col < gameBoard.Size;
        }

        //This method visits a cell on the grid. It will return true if the cell was a mine otherwise false. (ignores flagged cells)
        public bool MakeMove(int id)
        {
            //grab the current cell's position
            var row = id / gameBoard.Size;
            var col = id % gameBoard.Size;

            if (!withinBounds(row, col))
            {
                return false;
            }

            //check for flag
            if (gameBoard.Grid[row, col].Flagged)
            {
                return false;
            }

            //visit the cell
            gameBoard.Grid[row, col].Visited = true;

            //check for no neighbors
            if (gameBoard.Grid[row, col].LiveNeighbors == 0)
            {
                //recursively fill out all 0 neighbors cells
                floodFill(row, col);
            }

            return gameBoard.Grid[row, col].Mine;
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

                    //check if we have been to this cell already or if it is flagged
                    if (gameBoard.Grid[row + i, col + j].Visited || gameBoard.Grid[row + i, col + j].Flagged)
                    {
                        continue;
                    }

                    //visit the cell and check for continued recursion
                    gameBoard.Grid[row + i, col + j].Visited = true;

                    if (gameBoard.Grid[row + i, col + j].LiveNeighbors == 0)
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
