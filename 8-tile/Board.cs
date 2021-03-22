using System;
using System.Collections.Generic;
using System.Text;

namespace _8_tile
{
    class Board
    {
        public int[,] tiles;

        public Board(int[,] tiles)
        {
            this.tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }

        public Move[] GenerateCurrentBoardStateLegalMoves()
        {
            List<Move> moves = new List<Move>();

            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int cell = 0; cell < tiles.GetLength(1); cell++)
                {
                    foreach(Move M in GetLegalMovesForTile(row, cell))
                    {
                        if (!moves.Contains(M)) moves.Add(M);
                    }
                }
            }
            return moves.ToArray();
        }

        private Move[] GetLegalMovesForTile(int row, int cell)
        {
            List<Move> moves = new List<Move>();
  
            if(row + 1 < tiles.GetLength(0) && tiles[row+1,cell] == 0)
            {
                moves.Add(new Move(row, cell, Move.DIRECTION.DOWN));
            }
            
            if(row -1 >= 0 && tiles[row-1,cell] == 0)
            {
                moves.Add(new Move(row, cell, Move.DIRECTION.UP));
            }

            if (cell + 1 < tiles.GetLength(1) && tiles[row,cell+1] == 0)
            {
                moves.Add(new Move(row, cell, Move.DIRECTION.RIGHT));
            }

            if (cell - 1 >= 0 && tiles[row,cell - 1] == 0)
            {
                moves.Add(new Move(row, cell, Move.DIRECTION.LEFT));
            }
            return moves.ToArray();
        }

        private int CalculateTileValue(int row, int cell, int number)
        {
            Dictionary<int, int[]> positionDictionary = new Dictionary<int, int[]>()
            {
                {1, new int[]{0,0}},
                {2, new int[]{0,1}},
                {3, new int[]{0,2}},
                {4, new int[]{1,0}},
                {5, new int[]{1,1}},
                {6, new int[]{1,2}},
                {7, new int[]{2,0}},
                {8, new int[]{2,1}}
            };

            int[] myDesiredPosition = positionDictionary[number];
            int deltaX = Math.Abs(myDesiredPosition[1] - cell);
            int deltaY = Math.Abs(myDesiredPosition[0] - row);
            return deltaX + deltaY;
        }

        public int CalculateBoardValue()
        {
            int boardValue = 0;
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int cell = 0; cell < tiles.GetLength(1); cell++)
                {
                    if (tiles[row,cell] == 0) continue;
                    boardValue += CalculateTileValue(row, cell, tiles[row,cell]);
                }
            }
            return boardValue;
        }

        public Board GenerateBoardByExecutingMove(Move M)
        {
            int row = M.tileRow;
            int cell = M.tileCell;

            Move.DIRECTION direction = M.Direction;

            int targetRow = 0;
            int targetCell = 0;

            switch (direction)
            {
                case Move.DIRECTION.UP:
                    targetRow = row-1;
                    targetCell = cell;
                    break;
                case Move.DIRECTION.DOWN:
                    targetRow = row + 1;
                    targetCell = cell;
                    break;
                case Move.DIRECTION.LEFT:
                    targetRow = row;
                    targetCell = cell-1;
                    break; 
                case Move.DIRECTION.RIGHT:
                    targetRow = row;
                    targetCell = cell+1;
                    break; 
            }

            int targetValue = tiles[targetRow, targetCell];
            int sourceValue = tiles[row, cell];


            Board B = new Board(this.tiles.Clone() as int[,]);
            // swap
            B.tiles.SetValue(sourceValue, new int[] { targetRow, targetCell });
            B.tiles.SetValue(targetValue, new int[] { row, cell});

            return B;
        }

        public string RowToString(int row)
        {
            string s = "[";
            for(int cell = 0; cell < tiles.GetLength(1);  cell++)
            {
                if(tiles[row, cell] == 0)
                {
                    s += " ";
                }
                else
                {
                    s += tiles[row, cell];
                }
            }

            s += "]";
            return s;
        }

        public static string BoardArrayToString(Board[] boards, string extra = "")
        {
            string s = "";
            for (int i = 0; i < 3; i++)
            {
                foreach(Board B in boards)
                {
                    s += B.RowToString(i);
                    s += " ";
                }
                s += '\n';
            }
            return s;
        }

        public override string ToString()
        {
            string s = "";

            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                s += "[";
                for (int cell = 0; cell < tiles.GetLength(1); cell++)
                {
                    if (tiles[row, cell] == 0)
                    {
                        s += " ";
                    }
                    else
                    {
                        s += tiles[row, cell];
                    }
                }
                s += "]\n";
            }
            return s;
        }

        public override bool Equals(object obj)
        {
            Board other = obj as Board;
            return (other.ToString() == this.ToString());


        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

    }

    class Move
    {
        public enum DIRECTION
        { UP, DOWN, LEFT, RIGHT }

        public int tileRow, tileCell;
        public DIRECTION Direction;

        public Move(int row, int cell, DIRECTION dir)
        {
            this.tileRow = row;
            this.tileCell = cell;
            this.Direction = dir;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}] : {2}", tileRow, tileCell, Direction.ToString());
        }

        public string ToString(Board B)
        {
            string directionString;
            switch (Direction)
            {
                case DIRECTION.UP:
                    directionString = "up";
                    break;
                case DIRECTION.DOWN:
                    directionString = "down";
                    break;
                case DIRECTION.LEFT:
                    directionString = "left";
                    break;
                case DIRECTION.RIGHT:
                    directionString = "right";
                    break;
                default:
                    directionString = "";
                    break;
            }
            return string.Format("{0} {1}", B.tiles[tileRow, tileCell], directionString);
        }


    }
}
