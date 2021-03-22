using System;
using System.Collections.Generic;
using System.Text;

namespace vesikannut
{
    class GameState
    {
        public int Container_3 = 0;
        public int Container_5 = 0;

        public bool isComplete()
        {
            if (Container_5 == 4) return true;
            return false;
        }

        public void Reset()
        {
            Container_3 = 0;
            Container_5 = 0;
        }

        public override string ToString()
        {
            return "(" + Container_3 + ", " + Container_5 + ")";
        }

    }

    class Move
    {
        public Tuple<int, int> ContainerDelta;

        public Move(Tuple<int, int> containerDelta)
        {
            ContainerDelta = containerDelta ?? throw new ArgumentNullException(nameof(containerDelta));
        }

        public bool isLegal(GameState state)
        {
            int delta3 = ContainerDelta.Item1;
            int delta5 = ContainerDelta.Item2;


            // empty move
            if (delta3 == 0 && delta5 == 0) return false;
            
            // overflow underflow
            if (state.Container_3 + delta3 < 0 || state.Container_3 + delta3 > 3)
            {
                return false;
            }
            if (state.Container_5 + delta5 < 0 || state.Container_5 + delta5 > 5)
            {
                return false;
            }


            if(delta5 == 0)
            {
                if (state.Container_3 + delta3 != 3 && state.Container_3 + delta3 != 0) return false; 
            }


            if (delta3 == 0)
            {
                if (state.Container_5 + delta5 != 5 && state.Container_5 + delta5 != 0) return false;
            }


            // container -> container
            if (delta3 != 0 && delta5 != 0)
            {
                // conservation
                if (delta3 + delta5 != 0) return false;

                bool invalid = true;
                

                if(state.Container_5 + delta5 > 0 && state.Container_5 + delta5 < 5)
                {
                    if(state.Container_3 + delta3 > 0 && state.Container_3 + delta3 < 3)
                    {
                        return false;
                    }
                }



                //if (state.Container_3 + delta3 < 3 && state.Container_5 + delta5 < 5) return false;

                

            }

            return true;
        }


        public static Move[] GenerateLegalMoves(GameState state)
        {
            List<Move> allMoves = new List<Move>();

            for(int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    int delta3 = i - 3;
                    int delta5 = j - 5;



                    Move M = new Move(new Tuple<int, int>(delta3, delta5));
                    if (M.isLegal(state)) allMoves.Add(M);
                }
            }
            return allMoves.ToArray();
        }

        public void DestructiveUseMove(GameState oldState)
        {
            oldState.Container_3 += ContainerDelta.Item1;
            oldState.Container_5 += ContainerDelta.Item2;
        }

        public GameState ConstructiveUseMove(GameState state)
        {
            return new GameState()
            {
                Container_3 = state.Container_3 + ContainerDelta.Item1,
                Container_5 = state.Container_5 + ContainerDelta.Item2
            };
        }



        public override string ToString()
        {
            int delta3 = ContainerDelta.Item1;
            int delta5 = ContainerDelta.Item2;

            string S = "<" + delta3 + "," + delta5 + ">";
                
            return S;
        }

    }
}
