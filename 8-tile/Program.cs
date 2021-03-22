using System;
using System.Collections.Generic;

namespace _8_tile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("8-tile");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Board B = new Board(
                new int[,]
                {
                    {1,2,3},{4,6,8},{7,0,5}
                });


            HashSet<Board> seenBoards = new HashSet<Board>() { B };

            while (true)
            {
                //Console.Clear();
                if (!seenBoards.Contains(B)) seenBoards.Add(B);
                Console.WriteLine("\n ::::::: Board (value:" + B.CalculateBoardValue() + ") :::::::\n" + B.ToString() + '\n');
                //Console.WriteLine("Board value: " + B.CalculateBoardValue());


                Move[] moves = B.GenerateLegalMoves();

                List<Board> possibleBoards = new List<Board>();
                foreach(Move M in moves)
                {
                    possibleBoards.Add(B.ExecuteMove(M));
                }

                Console.WriteLine("Board possibilities:");
                Console.WriteLine(Board.BoardArrayToString(possibleBoards.ToArray()));

                Dictionary<int, Move> inputMappedMoves = new Dictionary<int, Move>();


                Console.WriteLine("Select a move with [number], write \"stop\" to Exit");
                for (int i = 0; i < moves.Length; i++)
                {
                    Board nB = B.ExecuteMove(moves[i]);
                    bool seenBefore = (seenBoards.Contains(nB));
                    Console.WriteLine("[{0}] move: {1}, heuristic value: {2} {3}",
                        i,
                        moves[i].ToString(B),
                        nB.CalculateBoardValue(),
                        (seenBefore) ? "(been before)" : ""
                        );
                    inputMappedMoves.Add(i, moves[i]);
                }

                string userInput = Console.ReadLine();
                if (userInput == "stop") break;

                if (int.TryParse(userInput, out int input))
                {
                    if (inputMappedMoves.ContainsKey(input))
                    {
                        Move m = inputMappedMoves[input];
                        Console.WriteLine("Chosen move: " + m.ToString(B));
                        B = B.ExecuteMove(m);
                    }
                }


            }
        }
    }
}
