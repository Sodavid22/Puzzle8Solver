using System.Diagnostics;

namespace Puzzle8Solver
{
    internal class PuzzleStep
    {
        public PuzzleStep Previous;
        public int[,] Matrix;

        public PuzzleStep(PuzzleStep previous, int[,] matrix)
        {
            Previous = previous;
            Matrix = matrix;
        }


        public void Draw()
        {
            Debug.Write("___");
            for (int x = 0; x < 3; x++)
            {
                Debug.WriteLine("");
                for (int y = 0; y < 3; y++)
                {
                    Debug.Write(Matrix[x, y]);
                }
            }
            Debug.WriteLine("");
        }


        public bool IsEqual(int[,] matrix)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (Matrix[x, y] != matrix[x, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
