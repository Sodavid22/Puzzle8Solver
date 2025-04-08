using System.Diagnostics;

namespace Puzzle8Solver
{
    internal class PuzzleStep
    {
        public PuzzleStep Previous;
        public int[] Matrix;

        public PuzzleStep(PuzzleStep previous, int[] matrix)
        {
            Previous = previous;
            Matrix = matrix;
            PuzzleManager.GeneratedSteps++;
        }


        public void Draw()
        {
            Debug.WriteLine("___");
            for (int i = 0; i < 9; i++)
            {
                if (i == 3 || i == 6)
                {
                    Debug.WriteLine("");
                }
                Debug.Write(Matrix[i]);
            }
            Debug.WriteLine("");
        }
        

        public bool IsEqual(int[] matrix)
        {
            for (int i = 0; i < 9; i++)
            {
                if (matrix[i] != Matrix[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
