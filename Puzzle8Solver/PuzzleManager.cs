using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Puzzle8Solver
{
    static class PuzzleManager
    {

        const int ButtonSize = 100;
        static Button[] Buttons = new Button[9];
        static List<PuzzleStep> AllSteps;
        static List<PuzzleStep> LastSteps;
        static List<PuzzleStep> NewSteps;
        public static List<PuzzleStep> FinalSteps;
        public static List<int> RemainingNumbers = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8 };
        public static int CurrentStep = 0;
        static int[] Input = new int[9];
        static int[] CurrentMatrix = new int[9];
        public static bool Solved = false;


        public static void Load()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Buttons[x*3+y] = new Button(new Rectangle(x * ButtonSize + Game.ButtonWidth + Game.ButtonSpacing*2, y * ButtonSize + Game.ButtonSpacing, ButtonSize, ButtonSize), Game.Self.buttoncolor, 1, "0");
                }
            }
        }


        public static void Update(float elapsedTime)
        {
            for (int i = 0; i < 9; i++)
            {
                Buttons[i].Text = CurrentMatrix[i].ToString();
                Buttons[i].Update(elapsedTime);

                if (Buttons[i].IsPressed(MouseKey.Left) && RemainingNumbers.Count > 0 && Input[i] == 0)
                {
                    Input[i] = RemainingNumbers[0];
                    RemainingNumbers.Remove(Input[i]);
                }
            }

            if (Solved)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        CurrentMatrix[x * 3 + y] = FinalSteps[CurrentStep].Matrix[y, x];
                    }
                }
            }
            else
            {
                CurrentMatrix = (int[])Input.Clone();
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
                Input[i] = RemainingNumbers[random.Next(0, RemainingNumbers.Count)];
                RemainingNumbers.Remove(Input[i]);
            }
            Solved = false;
            Game.Buttons[0].Color = Color.FromNonPremultiplied(Game.Self.buttoncolor);
        }


        public static void ClearInput()
        {
            Input = new int[9];
            RemainingNumbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            Solved = false;
            Game.Buttons[0].Color = Color.FromNonPremultiplied(Game.Self.buttoncolor);
        }


        public static void Solve()
        {
            AllSteps = new List<PuzzleStep>();
            LastSteps = new List<PuzzleStep>();
            NewSteps = new List<PuzzleStep>();
            FinalSteps = new List<PuzzleStep>();
            PuzzleStep finalStep = null;
            Solved = false;

            int[,] solution = new int[3, 3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };

            int[,] firstMatrix = new int[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    firstMatrix[y, x] = Input[x * 3 + y];
                }
            }

            AllSteps.Add(new PuzzleStep(null, firstMatrix));
            LastSteps.Add(AllSteps[0]);

            int generations = 0;
            Stopwatch sw = Stopwatch.StartNew();

            while (!Solved && generations < 32)
            {
                foreach (PuzzleStep step in LastSteps)
                {
                    CreateChilderen(step);
                }
                foreach (PuzzleStep step in NewSteps)
                {
                    if (step.IsEqual(solution))
                    {
                        Solved = true;
                        finalStep = step;
                        break;
                    }
                }

                LastSteps = NewSteps;
                NewSteps = new List<PuzzleStep>();
                generations++;
                Debug.WriteLine("generation: " + generations);
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
                FinalSteps.Add(AllSteps[0]);
                CurrentStep = 0;

                Game.Buttons[0].Color = Color.LawnGreen;
                Debug.WriteLine("SOLVED");
                Debug.WriteLine("positions tried: " + AllSteps.Count);
                Debug.WriteLine("time: " + sw.Elapsed);
            }
            else
            {
                Game.Buttons[0].Color = Color.Orange;
                Debug.WriteLine("FAILED");
                Debug.WriteLine("positions tried: " + AllSteps.Count);
                Debug.WriteLine("time: " + sw.Elapsed);
            }
        }

        public static void CreateChilderen(PuzzleStep step)
        {
            int[,] newMatrix;
            PuzzleStep newStep;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (step.Matrix[x, y] == 0)
                    {
                        if (x < 2) // move zero right
                        {
                            newMatrix = (int[,])step.Matrix.Clone();

                            newMatrix[x, y] = newMatrix[x + 1, y];
                            newMatrix[x + 1, y] = 0;

                            if (!IsDuplicate(newMatrix)) 
                            {
                                newStep = new PuzzleStep(step, newMatrix);
                                AllSteps.Add(newStep);
                                NewSteps.Add(newStep);
                            }
                        }
                        if (x > 0) // move zero left
                        {
                            newMatrix = (int[,])step.Matrix.Clone();

                            newMatrix[x, y] = newMatrix[x - 1, y];
                            newMatrix[x - 1, y] = 0;

                            if (!IsDuplicate(newMatrix))
                            {
                                newStep = new PuzzleStep(step, newMatrix);
                                AllSteps.Add(newStep);
                                NewSteps.Add(newStep);
                            }
                        }
                        if (y > 0) // move zero up
                        {
                            newMatrix = (int[,])step.Matrix.Clone();

                            newMatrix[x, y] = newMatrix[x, y - 1];
                            newMatrix[x, y - 1] = 0;

                            if (!IsDuplicate(newMatrix))
                            {
                                newStep = new PuzzleStep(step, newMatrix);
                                AllSteps.Add(newStep);
                                NewSteps.Add(newStep);
                            }
                        }
                        if (y < 2) // move zero down
                        {
                            newMatrix = (int[,])step.Matrix.Clone();

                            newMatrix[x, y] = newMatrix[x, y + 1];
                            newMatrix[x, y + 1] = 0;

                            if (!IsDuplicate(newMatrix))
                            {
                                newStep = new PuzzleStep(step, newMatrix);
                                AllSteps.Add(newStep);
                                NewSteps.Add(newStep);
                            }
                        }
                        return;
                    }
                }
            }
        }

        public static bool IsDuplicate(int[,] matrix)
        {
            foreach (PuzzleStep comparedStep in AllSteps)
            {
                if (comparedStep.IsEqual(matrix))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
