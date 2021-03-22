using System;
using System.Collections.Generic;

namespace _8_tile
{
    class Program
    {
        static bool clearBoardEveryRound = false;
        static Board gameBoard;
        static HashSet<Board> seenBoards;

        static void Main(string[] args)
        {
            InitalizeGame();

            while (true)
            {
                if (clearBoardEveryRound) Console.Clear();
                if (!seenBoards.Contains(gameBoard)) seenBoards.Add(gameBoard);
                Move[] moves = gameBoard.GenerateCurrentBoardStateLegalMoves();

                PrintCurrentBoardInfo(moves);

                Dictionary<int, Move> inputMappedMoves = GenerateInputMappedMoves(moves);
                PrintUserInputPrompt(inputMappedMoves);

                string userInput = UserInput(inputMappedMoves);
                if (userInput == "stop") break;
            }
        }

        static bool QueryUserAboutBoardClearing()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("[1]: Clear console after every move");
            Console.WriteLine("[2]: Don't clear console");
            while (true)
            {
                string userInputChar = "" + Console.ReadKey().KeyChar;
                if (int.TryParse(userInputChar, out int userInputInt))
                {
                    if (userInputInt == 1)
                    {
                        clearBoardEveryRound = true;
                        break;
                    }
                    else if (userInputInt == 2)
                    {
                        clearBoardEveryRound = false;
                        break;
                    }
                }
            }
            return clearBoardEveryRound;
        }

        static void InitalizeGame()
        {
            Console.WriteLine("8-tile");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            gameBoard = new Board(
                new int[,]
                {
                    {1,2,3},{4,6,8},{7,0,5}
                });


            seenBoards = new HashSet<Board>() { gameBoard };
            clearBoardEveryRound = QueryUserAboutBoardClearing();
        }

        static void PrintCurrentBoardInfo(Move[] currentLegalMoves)
        {
            Console.WriteLine("::::::: Board (value:" + gameBoard.CalculateBoardValue() + ") :::::::\n" + gameBoard.ToString());
            List<Board> possibleBoards = new List<Board>();
            foreach (Move M in currentLegalMoves)
            {
                possibleBoards.Add(gameBoard.GenerateBoardByExecutingMove(M));
            }
            Console.WriteLine("Board possibilities:");
            Console.WriteLine(Board.BoardArrayToString(possibleBoards.ToArray()));

        }

        static Dictionary<int, Move> GenerateInputMappedMoves(Move[] currentLegalMoves)
        {
            Dictionary<int, Move> inputMappedMoves = new Dictionary<int, Move>();
            for (int i = 0; i < currentLegalMoves.Length; i++)
            {
                inputMappedMoves.Add(i, currentLegalMoves[i]);
            }
            return inputMappedMoves;
        }

        static void PrintUserInputPrompt(Dictionary<int,Move> inputMappedMoves)
        {
            Console.WriteLine("Select a move with [number], write \"stop\" to Exit");

            foreach(KeyValuePair<int, Move> mappedMove in inputMappedMoves)
            {
                Board nB = gameBoard.GenerateBoardByExecutingMove(mappedMove.Value);
                bool seenBefore = (seenBoards.Contains(nB));
                Console.WriteLine("[{0}] Move {1}, heuristic value: {2} {3}",
                    mappedMove.Key,
                    mappedMove.Value.ToString(gameBoard),
                    nB.CalculateBoardValue(),
                    (seenBefore) ? "(been before)" : ""
                    );
            }
            Console.WriteLine("[{0}] Exit game", "stop");

        }

        static string UserInput(Dictionary<int, Move> inputMappedMoves)
        {
            string userInput = Console.ReadLine();

            if (userInput == "stop") return "stop";

            if (int.TryParse(userInput, out int input))
            {
                if (inputMappedMoves.ContainsKey(input))
                {
                    Move m = inputMappedMoves[input];
                    Console.WriteLine("Chosen move: " + m.ToString(gameBoard));
                    gameBoard = gameBoard.GenerateBoardByExecutingMove(m);
                }
            }

            return userInput;
        }
    }
}
