using Microsoft.Xna.Framework;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Puzzle8Solver
{
    static class PuzzleManager
    {

        const int ButtonSize = 100;
        static Button[] Buttons = new Button[9];
        static List<PuzzleStep> PreviousGeneration;
        static List<PuzzleStep> NewGeneration;
        public static List<PuzzleStep> FinalSteps;
        public static Hashtable TestedStepsHashed;
        public static List<int> RemainingNumbers = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8 };
        public static int CurrentStep = 0;
        static int[] StartingMatrix = new int[9];
        static int[] DisplayedMatrix = new int[9];
        public static bool Solved = false;
        public static int GeneratedSteps = 0;


        public static void Load()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Buttons[y*3+x] = new Button(new Rectangle(x * ButtonSize + Game.ButtonWidth + Game.ButtonSpacing*2, y * ButtonSize + Game.ButtonSpacing, ButtonSize, ButtonSize), Game.Self.buttoncolor, 1, "0");
                }
            }
        }


        public static void Update(float elapsedTime)
        {
            for (int i = 0; i < 9; i++)
            {
                Buttons[i].Text = DisplayedMatrix[i].ToString();
                Buttons[i].Update(elapsedTime);

                if (Buttons[i].IsPressed(MouseKey.Left) && RemainingNumbers.Count > 0 && StartingMatrix[i] == 0)
                {
                    StartingMatrix[i] = RemainingNumbers[0];
                    RemainingNumbers.Remove(StartingMatrix[i]);
                }
            }

            if (Solved)
            {
                DisplayedMatrix = (int[])FinalSteps[CurrentStep].Matrix.Clone();
            }
            else
            {
                DisplayedMatrix = (int[])StartingMatrix.Clone();
            }
        }


        public static void Draw()
        {
            foreach (var button in Buttons)
            {
                button.Draw();
            }
        }


        public static void GenerateMatrix()
        {
            RemainingNumbers = new List<int> () {0,1,2,3,4,5,6,7,8};
            Random random = new Random();

            for (int i = 0; i < 9; i++)
            {
                StartingMatrix[i] = RemainingNumbers[random.Next(0, RemainingNumbers.Count)];
                RemainingNumbers.Remove(StartingMatrix[i]);
            }
            Solved = false;
            Game.Buttons[0].Color = Color.FromNonPremultiplied(Game.Self.buttoncolor);
        }


        public static void ClearInput()
        {
            StartingMatrix = new int[9];
            RemainingNumbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            Solved = false;
            Game.Buttons[0].Color = Color.FromNonPremultiplied(Game.Self.buttoncolor);
        }


        public static void Solve()
        {
            GeneratedSteps = 0;
            PreviousGeneration = new List<PuzzleStep>();
            NewGeneration = new List<PuzzleStep>();
            FinalSteps = new List<PuzzleStep>();
            TestedStepsHashed = new Hashtable();
            PuzzleStep finalStep = null;
            Solved = false;

            int[] solution = new int[9] { 0,1,2,3,4,5,6,7,8 };

            PuzzleStep firstStep = new PuzzleStep(null, StartingMatrix);
            TestedStepsHashed.Add(firstStep.ToString(), firstStep);
            PreviousGeneration.Add(firstStep);

            int generations = 0;
            Stopwatch sw = Stopwatch.StartNew();

            if (firstStep.IsEqual(solution))
            {
                Solved = true;
                finalStep = firstStep;
            }

            while (!Solved && generations < 31)
            {
                foreach (PuzzleStep step in PreviousGeneration)
                {
                    CreateChilderen(step);
                }
                foreach (PuzzleStep step in NewGeneration)
                {
                    if (step.IsEqual(solution))
                    {
                        Solved = true;
                        finalStep = step;
                        break;
                    }
                }

                PreviousGeneration = NewGeneration;
                NewGeneration = new List<PuzzleStep>();
                generations++;
            }

            sw.Stop();

            if (Solved)
            {
                PuzzleStep currentStep = finalStep;
                while (currentStep.Previous != null)
                {
                    FinalSteps.Add(currentStep);
                    currentStep = currentStep.Previous;
                }
                FinalSteps.Add(firstStep);
                CurrentStep = 0;

                Game.Buttons[0].Color = Color.LawnGreen;
                Debug.WriteLine("SOLVED");
                Debug.WriteLine("time: " + sw.Elapsed);
                Debug.WriteLine("moves to solve: " + (FinalSteps.Count - 1));
                Debug.WriteLine("generated positions: " + GeneratedSteps + " (" + (GeneratedSteps / 181440f)*100 + "%)");
            }
            else
            {
                Game.Buttons[0].Color = Color.Orange;
                Debug.WriteLine("FAILED");
                Debug.WriteLine("time: " + sw.Elapsed);
                Debug.WriteLine("moves to solve: " + (FinalSteps.Count - 1));
                Debug.WriteLine("generated positions: " + GeneratedSteps + " (" + (GeneratedSteps / 181440f)*100 + "%)");
            }
        }

        public static void CreateChilderen(PuzzleStep step)
        {
            int[] newMatrix;

            for (int i = 0; i < 9; i++)
            {
                if (step.Matrix[i] == 0)
                {
                    if (i!= 2 && i!= 5 && i!= 8) // move zero right
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i+1];
                        newMatrix[i+1] = 0;

                        SaveIfNew(step, newMatrix);
                    }
                    if (i != 0 && i != 3 && i != 6) // move zero left
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i-1];
                        newMatrix[i-1] = 0;

                        SaveIfNew(step, newMatrix);
                    }
                    if (i > 2) // move zero up
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i-3];
                        newMatrix[i-3] = 0;

                        SaveIfNew(step, newMatrix);
                    }
                    if (i < 6) // move zero down
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i+3];
                        newMatrix[i+3] = 0;

                        SaveIfNew(step, newMatrix);
                    }
                    return;
                }
            }
        }

        public static void SaveIfNew(PuzzleStep step, int[] matrix)
        {
            if (!TestedStepsHashed.Contains(MatrixToString(matrix)))
            {
                PuzzleStep newStep = new PuzzleStep(step, matrix);
                NewGeneration.Add(newStep);
                TestedStepsHashed.Add(MatrixToString(matrix), newStep);
            }
        }


        public static string MatrixToString(int[] Matrix)
        {
            return string.Join("", Matrix); ;
        }
    }
}
