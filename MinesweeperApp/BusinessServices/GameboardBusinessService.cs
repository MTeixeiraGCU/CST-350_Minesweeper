using MinesweeperApp.DatabaseServices;
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
                return GameBoard.Size;
            }
        }
        public Board GameBoard { get; set; }

        public delegate void RevealCell(int id);

        private const int GRID_SIZE = 10;
        private float[] DifficultySettings = { 0.12f, 0.14f, 0.16f };

        public List<Cell> Grid
        {
            get
            {
                List<Cell> cells = new List<Cell>();
                for (int i = 0; i < GameBoard.Size; i++)
                {
                    for (int j = 0; j < GameBoard.Size; j++)
                    {
                        cells.Add(GameBoard.Grid[i, j]);
                    }
                }
                return cells;
            }
        }


        public GameboardBusinessService()
        {
            GameBoard = new Board();
        }

        //This method initiates a new game and randomizes the mines on the grid based off of the difficulty
        public void NewGame(string difficulty)
        {
            //create a new board
            GameBoard.Difficulty = getDifficultyFromString(difficulty);
            GameBoard.Size = GRID_SIZE;
            GameBoard.TimeStarted = GameBoard.CurrentStartTime = DateTime.Now;
            
            //clears the existing grid to a certain size
            clearGrid(GameBoard.Size);

            //create the mines on the board
            setupLiveNeighbors();

            //determine all the neighbor counts on the new board
            calculateLiveNeighbors();
        }

        //This method toggles the flagged status of a given cell
        public void ToggleFlag(int id)
        {
            //grab the current cell's position
            var row = id / GameBoard.Size;
            var col = id % GameBoard.Size;

            //check bounds first
            if (withinBounds(row, col))
            {
                //toggle flag
                GameBoard.Grid[row, col].Flagged = !GameBoard.Grid[row, col].Flagged;
            }
        }

        // This method checks the entire board for all visits to determine endgame status
        public bool CheckForWin()
        {
            int counter = 0;

            foreach (var cell in GameBoard.Grid)
            {
                if (cell.Visited || (cell.Flagged && cell.Mine))
                {
                    counter++;
                }
            }

            if (counter == (GameBoard.Size * GameBoard.Size))
                return true;
            else
                return false;
        }

        //This method visits a cell on the grid. It will return true if the cell was a mine otherwise false. (ignores flagged cells)
        public bool MakeMove(int id, RevealCell revealCell = null)
        {
            //grab the current cell's position
            var row = id / GameBoard.Size;
            var col = id % GameBoard.Size;

            if (!withinBounds(row, col))
            {
                return false;
            }

            //check for flag or a previous visit
            if (GameBoard.Grid[row, col].Flagged || GameBoard.Grid[row, col].Visited)
            {
                return false;
            }

            //visit the cell
            GameBoard.Grid[row, col].Visited = true;
            if(revealCell != null)
                revealCell((row * GameBoard.Size) + col);

            //check for no neighbors
            if (GameBoard.Grid[row, col].LiveNeighbors == 0)
            {
                //recursively fill out all 0 neighbors cells
                floodFill(row, col, revealCell);
            }

            return GameBoard.Grid[row, col].Mine;
        }

        public void RevealAll(RevealCell revealCell)
        {
            foreach (var cell in GameBoard.Grid)
            {
                if (!cell.Visited)
                {
                    cell.Visited = true;
                    revealCell(cell.Id);
                }
            }
        }

        public DateTime GetStartTime()
        {
            return GameBoard.TimeStarted;
        }

        //This method clears the given board grid and sizes the new one based on the inputted size
        private void clearGrid(int size)
        {
            GameBoard.Grid = new Cell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //Id for each cell will be (row * width + col)
                    GameBoard.Grid[i, j] = new Cell((i * size) + j);
                }
            }
        }

        //This method sets up the game board with mines based on the current difficulty
        private void setupLiveNeighbors()
        {
            //random number generator
            var rand = new Random();

            //first determine the number of mines based on difficulty
            GameBoard.NumberOfMines = (int)((GameBoard.Size * GameBoard.Size) * DifficultySettings[GameBoard.Difficulty - 1]);

            for (int i = 0; i < GameBoard.NumberOfMines; i++)
            {
                //randomly place mine
                var row = rand.Next(0, GameBoard.Size);
                var col = rand.Next(0, GameBoard.Size);

                //check for previously placed mine
                if (GameBoard.Grid[row, col].Mine)
                {
                    //go back and try to place it again
                    i--;
                }
                else
                {
                    GameBoard.Grid[row, col].Mine = true;
                }
            }
        }

        //This method goes through each cell and calculates how many mines it is touching
        private void calculateLiveNeighbors()
        {
            //loop through each Cell to populate its neighbors
            foreach (var cell in GameBoard.Grid)
            {
                //loop through each cell surrounding the current cell in a 3x3 Grid
                for (int m = -1; m < 2; m++)
                {
                    for (int n = -1; n < 2; n++)
                    {
                        //grab the current neighbor's position
                        var row = (cell.Id / GameBoard.Size) + m;
                        var col = (cell.Id % GameBoard.Size) + n;

                        //check if we are off the board
                        if (!withinBounds(row, col))
                        {
                            continue;
                        }

                        //check for mine
                        if (GameBoard.Grid[row, col].Mine)
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
            return row >= 0 && col >= 0 && row < GameBoard.Size && col < GameBoard.Size;
        }

        //Recursive function to find all empty neighbors and set them to visited
        private void floodFill(int row, int col, RevealCell revealCell = null)
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
                    if (GameBoard.Grid[row + i, col + j].Visited || GameBoard.Grid[row + i, col + j].Flagged)
                    {
                        continue;
                    }

                    //visit the cell and check for continued recursion
                    GameBoard.Grid[row + i, col + j].Visited = true;
                    if(revealCell != null)
                        revealCell(((row + i) * GameBoard.Size) + col + j);

                    if (GameBoard.Grid[row + i, col + j].LiveNeighbors == 0)
                    {
                        floodFill(row + i, col + j, revealCell);
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
