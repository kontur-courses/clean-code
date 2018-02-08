using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class TemporaryPieceMove : IDisposable
    {
        private readonly Board board;
        private readonly Location from;
        private readonly Piece oldDestinationPiece;
        private readonly Location to;

        public TemporaryPieceMove(Board board, Location from, Location to, Piece oldDestinationPiece)
        {
            this.board = board;
            this.from = from;
            this.to = to;
            this.oldDestinationPiece = oldDestinationPiece;
        }

        public void Undo()
        {
            board.Set(from, board.GetPiece(to));
            board.Set(to, oldDestinationPiece);
        }

        public void Dispose()
        {
            Undo();
        }
    }
}