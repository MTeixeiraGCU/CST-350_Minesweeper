using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.DatabaseServices
{
    /// <summary>
    /// This interface represents a persistence layer connection to create, add, update and delete a Gameboard model.
    /// </summary>
    public interface IGameBoardDAO
    {
        public List<Board> GetAll();
        public List<Board> GetFromUserId(int userId);
        public int Add(Board board, int userId);
        public bool Update(Board board);
        public Board Get(int boardId);
        public bool Delete(int boardId);

    }
}
