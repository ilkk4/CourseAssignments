using System;
using System.Collections.Generic;

namespace vesikannut
{
    class Program
    {
        public static int CURRENT_ITERATION = 0;
        public const int MAX_ITERATIONS = 1000;
        public const int MAX_DEPTH = 100;

        static void Main(string[] args)
        {
            Console.WriteLine(" ::: Containers (5 & 3) ::: ");

            GameState game = new GameState();


            while(true)
            {
                Console.Clear();
                Console.WriteLine("GAMESTATE");

                Console.WriteLine(game.ToString());

                Console.WriteLine("Available moves:");

                Move[] moves = Move.GenerateLegalMoves(game);
                Dictionary<int, Move> inputMappedMoves = new Dictionary<int, Move>();
                


                for (int i = 0; i < moves.Length; i++)
                {
                    Console.WriteLine("[{0}] {1} -> {2}", i, moves[i].ToString(), moves[i].ConstructiveUseMove(game).ToString());
                    inputMappedMoves.Add(i, moves[i]);
                }

                string userInput = Console.ReadLine();
                if (userInput == "stop") break;

                if(int.TryParse(userInput, out int input))
                {
                    if (inputMappedMoves.ContainsKey(input))
                    {
                        inputMappedMoves[input].DestructiveUseMove(game);
                    }
                }


            }



        }

        static bool dfs(GameState state, int depth, HashSet<string> seenStates)
        {
            if (seenStates.Contains(state.ToString())) return false;


            if (state.isComplete())
            {
                Console.WriteLine("Game complete!!");
                return true;
            }

            if (CURRENT_ITERATION > MAX_ITERATIONS) return false;
            if (depth > MAX_DEPTH) return false;

            Move[] legalMoves = Move.GenerateLegalMoves(state);
            Console.WriteLine("\n depth:"+depth + "> Game state:" + state.ToString() +  " iter:" + CURRENT_ITERATION);
            Console.WriteLine("Legal moves:");
            foreach (Move M in legalMoves)
            {
                Console.WriteLine(M.ToString() + (seenStates.Contains(M.ConstructiveUseMove(state).ToString()) ? " seen " : ""));
            }

            if (legalMoves.Length == 0)
            {
                Console.WriteLine("No legal moves");
                return false;
            }
            else
            {
                foreach(Move M in legalMoves)
                {
                    GameState newState = M.ConstructiveUseMove(state);
                    if (seenStates.Contains(M.ConstructiveUseMove(state).ToString())) return false;
                    CURRENT_ITERATION += 1;

                    bool found = dfs(M.ConstructiveUseMove(state), depth +1, seenStates);
                    if (found) return true;
                }
                seenStates.Add(state.ToString());
                return false;
            }
        }

    }
}
