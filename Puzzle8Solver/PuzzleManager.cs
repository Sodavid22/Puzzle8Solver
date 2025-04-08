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
        static int[] DisplayedMatrix = new int[9];
        public static bool Solved = false;


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

                if (Buttons[i].IsPressed(MouseKey.Left) && RemainingNumbers.Count > 0 && Input[i] == 0)
                {
                    Input[i] = RemainingNumbers[0];
                    RemainingNumbers.Remove(Input[i]);
                }
            }

            if (Solved)
            {
                DisplayedMatrix = (int[])FinalSteps[CurrentStep].Matrix.Clone();
            }
            else
            {
                DisplayedMatrix = (int[])Input.Clone();
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

            int[] solution = new int[9] { 0,1,2,3,4,5,6,7,8 };

            PuzzleStep firstStep = new PuzzleStep(null, Input);
            AllSteps.Add(firstStep);
            LastSteps.Add(firstStep);

            int generations = 0;
            Stopwatch sw = Stopwatch.StartNew();

            if (firstStep.IsEqual(solution))
            {
                Solved = true;
                finalStep = firstStep;
            }

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
                Debug.WriteLine("childeren: " + LastSteps.Count);
                Debug.WriteLine("total: " + AllSteps.Count);
                Debug.WriteLine("---------------");
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
            int[] newMatrix;
            PuzzleStep newStep;

            for (int i = 0; i < 9; i++)
            {
                if (step.Matrix[i] == 0)
                {
                    if (i!= 2 && i!= 5 && i!= 8) // move zero right
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i+1];
                        newMatrix[i+1] = 0;

                        if (!IsDuplicate(newMatrix)) 
                        {
                            newStep = new PuzzleStep(step, newMatrix);
                            AllSteps.Add(newStep);
                            NewSteps.Add(newStep);
                        }
                    }
                    if (i != 0 && i != 3 && i != 6) // move zero left
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i-1];
                        newMatrix[i-1] = 0;

                        if (!IsDuplicate(newMatrix))
                        {
                            newStep = new PuzzleStep(step, newMatrix);
                            AllSteps.Add(newStep);
                            NewSteps.Add(newStep);
                        }
                    }
                    if (i > 2) // move zero up
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i-3];
                        newMatrix[i-3] = 0;

                        if (!IsDuplicate(newMatrix))
                        {
                            newStep = new PuzzleStep(step, newMatrix);
                            AllSteps.Add(newStep);
                            NewSteps.Add(newStep);
                        }
                    }
                    if (i < 6) // move zero down
                    {
                        newMatrix = (int[])step.Matrix.Clone();

                        newMatrix[i] = newMatrix[i+3];
                        newMatrix[i+3] = 0;

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

        public static bool IsDuplicate(int[] matrix)
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
