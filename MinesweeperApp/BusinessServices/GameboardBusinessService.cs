using MinesweeperApp.DatabaseServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.BusinessServices
{
    /// <summary>
    /// This business service class is designed to handle in game logic for a minsweeper board.
    /// </summary>
    public class GameboardBusinessService
    {
        //The square size of the game board
        public int Size {
            get {
                return GameBoard.Size;
            }
        }

        //The game board object of the current game
        public Board GameBoard { get; set; }

        //callback method signature for revealing a single Cell
        public delegate void RevealCell(int id);

        //Constant grid size value for creating a new game 
        private const int GRID_SIZE = 10;

        //Local array of values to determine mine count on each new board.
        private float[] DifficultySettings = { 0.12f, 0.14f, 0.16f };

        /// <summary>
        /// A single dimensional List of Cell objects that represent the current board.
        /// </summary>
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

        /// <summary>
        /// This method initiates a new game and randomizes the mines on the grid based off of the difficulty
        /// </summary>
        /// <param name="difficulty">A string representing a value of (Easy, Medium, Hard) will default to Medium.</param>
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

        /// <summary>
        /// This method toggles the flagged status of a given cell
        /// </summary>
        /// <param name="cellId">The given cell Id to toggle the flag of.</param>
        public void ToggleFlag(int cellId)
        {
            //grab the current cell's position
            var row = cellId / GameBoard.Size;
            var col = cellId % GameBoard.Size;

            //check bounds first
            if (withinBounds(row, col))
            {
                //toggle flag
                GameBoard.Grid[row, col].Flagged = !GameBoard.Grid[row, col].Flagged;
            }
        }

        /// <summary>
        /// This method checks the entire board for all visits to determine endgame status
        /// </summary>
        /// <returns>true if endgame status was achieved, false otherwise.</returns>
        public bool CheckForWin()
        {
            //counter to track the number of cells checked.
            int counter = 0;

            //go through each cell to check its status
            foreach (var cell in GameBoard.Grid)
            {
                //If the cell was visited or the cell was flagged and a mine add one to the counter.
                if (cell.Visited || (cell.Flagged && cell.Mine))
                {
                    counter++;
                }
            }

            //If the counter reached all cells in the grid we have won.
            if (counter == (GameBoard.Size * GameBoard.Size))
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method visits a cell on the grid. Will recursivly visit neighbors when appropriate. (ignores flagged cells)
        /// </summary>
        /// <param name="cellId">The Id of the cell to visit initially.</param>
        /// <param name="revealCell">A callback method of type RevealCell to help with revealing logic of each cell visited in cases of recursion.</param>
        /// <returns>It will return true if the cell was a mine otherwise false.</returns>
        public bool MakeMove(int cellId, RevealCell revealCell = null)
        {
            //grab the current cell's position
            var row = cellId / GameBoard.Size;
            var col = cellId % GameBoard.Size;

            //check that we are inside the grid still
            if (!withinBounds(row, col))
            {
                return false;
            }

            //check for flag or a previous visit
            if (GameBoard.Grid[row, col].Flagged || GameBoard.Grid[row, col].Visited)
            {
                return false;
            }

            //visit the cell and call reveal logic on it
            GameBoard.Grid[row, col].Visited = true;
            if(revealCell != null)
                revealCell((row * GameBoard.Size) + col);

            //check for no neighbors
            if (GameBoard.Grid[row, col].LiveNeighbors == 0)
            {
                //recursively fill out all connected no neighbor cells
                floodFill(row, col, revealCell);
            }

            return GameBoard.Grid[row, col].Mine;
        }

        /// <summary>
        /// This method helps with revealing all cells on the grid in cases of a loss.
        /// </summary>
        /// <param name="revealCell">A callback method to allow outside revealing cell logic.</param>
        public void RevealAll(RevealCell revealCell)
        {
            //loop through all of the cells
            foreach (var cell in GameBoard.Grid)
            {
                if (!cell.Visited)
                {
                    //reveal the cell and callback to revealing logic
                    cell.Visited = true;
                    revealCell(cell.Id);
                }
            }
        }

        /// <summary>
        /// This method is a getter method for the initial start time of the game.
        /// </summary>
        /// <returns>The current games initial start time.</returns>
        public DateTime GetStartTime()
        {
            return GameBoard.TimeStarted;
        }

        /// <summary>
        /// This method clears the given board grid and sizes the new one based on the input size
        /// </summary>
        /// <param name="size">The new grids size.</param>
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

        /// <summary>
        /// This method sets up the game board with mines based on the current difficulty
        /// </summary>
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
                    //place the new mine
                    GameBoard.Grid[row, col].Mine = true;
                }
            }
        }

        /// <summary>
        /// This method goes through each cell and calculates how many mines it is touching
        /// </summary>
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
                            //increment the touching mine count
                            cell.LiveNeighbors++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method checks that we are inside the bounds
        /// </summary>
        /// <param name="row">The row of the position to check for.</param>
        /// <param name="col">The column of the position to check for.</param>
        /// <returns>true if the given position is outside the current size grid, false otherwise.</returns>
        private bool withinBounds(int row, int col)
        {
            return row >= 0 && col >= 0 && row < GameBoard.Size && col < GameBoard.Size;
        }

        /// <summary>
        /// Recursive function to find all adjacent zero live neighbor cells and set them to visited
        /// </summary>
        /// <param name="row">The row of the current cell we are checking connections with.</param>
        /// <param name="col">The col of the current cell we are checkgin connections with.</param>
        /// <param name="revealCell">A callback method to help with outside revealing logic.</param>
        private void floodFill(int row, int col, RevealCell revealCell = null)
        {
            //loop through all of the 3 x 3 neighbors of the current cell
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

                    //visit the cell then callback to any reveal logic
                    GameBoard.Grid[row + i, col + j].Visited = true;
                    if(revealCell != null)
                        revealCell(((row + i) * GameBoard.Size) + col + j);

                    //check for continued recursion
                    if (GameBoard.Grid[row + i, col + j].LiveNeighbors == 0)
                    {
                        floodFill(row + i, col + j, revealCell);
                    }
                }
            }
        }

        /// <summary>
        /// This method gets the difficulty based on the string name of each level . Will defualt to 2 if no difficulty was parsed.
        /// </summary>
        /// <param name="difficutly">String representing a given difficulty.</param>
        /// <returns>The difficulty as an integer between (1 - 3)</returns>
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
