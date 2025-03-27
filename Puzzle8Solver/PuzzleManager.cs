using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Puzzle8Solver
{
    static class PuzzleManager
    {
        static List<int> Input = new List<int>();


        public static void Update(float elapsedTime)
        {
            UpdateEntryBox();
        }


        public static void UpdateEntryBox()
        {
            Button entryField = Game.Buttons[2];
            int key = -1;
            string text = "";

            if (entryField.Hovered)
            {
                if (MyKeyboard.IsPressed(Keys.NumPad0)) { key = 0; }
                if (MyKeyboard.IsPressed(Keys.NumPad1)) { key = 1; }
                if (MyKeyboard.IsPressed(Keys.NumPad2)) { key = 2; }
                if (MyKeyboard.IsPressed(Keys.NumPad3)) { key = 3; }
                if (MyKeyboard.IsPressed(Keys.NumPad4)) { key = 4; }
                if (MyKeyboard.IsPressed(Keys.NumPad5)) { key = 5; }
                if (MyKeyboard.IsPressed(Keys.NumPad6)) { key = 6; }
                if (MyKeyboard.IsPressed(Keys.NumPad7)) { key = 7; }
                if (MyKeyboard.IsPressed(Keys.NumPad8)) { key = 8; }
            }

            if (key >= 0 && Input.Count < 9 && !Input.Contains(key))
            {
                Input.Add(key);
            }

            if (MyKeyboard.IsPressed(Keys.Back) && Input.Count > 0)
            {
                Input.RemoveAt(Input.Count - 1);
            }

            for (int i = 0; i < Input.Count; i++)
            {
                text = text + Input[i].ToString();
            }

            Game.Buttons[2].Text = text;
        }
    }
}
