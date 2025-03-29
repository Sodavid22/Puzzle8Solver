using System.Diagnostics;

namespace Puzzle8Solver
{
    internal class PuzzleStep
    {
        public PuzzleStep Previous;
        public byte[] Matrix;

        public PuzzleStep(PuzzleStep previous, byte[] matrix)
        {
            Previous = previous;
            Matrix = matrix;
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


        public bool IsEqual(byte[] matrix)
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
