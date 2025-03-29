using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Puzzle8Solver
{
    public class Button
    {
        public Rectangle Rectangle;
        public Color Color;
        public int Border;
        Vector2 TextSize;
        public string Text;
        public bool Hovered;
        public bool Pressed;
        public float Flash;
        public bool Active = true;


        public Button(Rectangle rectangle, Vector4 color, int border, string text)
        {
            Rectangle = rectangle;
            Color = Color.FromNonPremultiplied(color);
            Border = border;
            Text = text;
            Flash = -1;
        }


        public virtual void Update(float elapsedTime)
        {
            Hovered = false;
            Pressed = false;
            Vector2 mousePosition = MyKeyboard.GetMousePosition();

            if (mousePosition.X > Rectangle.X && mousePosition.X < Rectangle.X + Rectangle.Width)
            {
                if (mousePosition.Y > Rectangle.Y && mousePosition.Y < Rectangle.Y + Rectangle.Height)
                {
                    Hovered = true;
                }
            }

            if (Hovered && MyKeyboard.IsPressed(MouseKey.Left))
            {
                Pressed = true;
                Flash = 0.1f;
            }

            Flash -= elapsedTime;

            TextSize = Game.Font.MeasureString(Text);
        }


        public virtual void Draw()
        {
            Game.SpriteBatch.Draw(Textures.EmptyTexture, new Rectangle(Rectangle.X - Border, Rectangle.Y - Border, Rectangle.Width + Border * 2, Rectangle.Height + Border * 2), Color.Black);
            Game.SpriteBatch.Draw(Textures.EmptyTexture, Rectangle, Color);
            Game.SpriteBatch.DrawString(Game.Font, Text, new Vector2((int)(Rectangle.X + (Rectangle.Width/2) - TextSize.X / 2), (int)(Rectangle.Y + (Rectangle.Height / 2) - TextSize.Y / 2)), Color.Black);

            if (Hovered)
            {
                Game.SpriteBatch.Draw(Textures.EmptyTexture, Rectangle, Color.FromNonPremultiplied(new Vector4(0, 0, 0, 0.1f)));
            }
            if (Pressed && Active || Flash > 0 && Active)
            {
                Game.SpriteBatch.Draw(Textures.EmptyTexture, Rectangle, Color.FromNonPremultiplied(new Vector4(0, 0, 0, 0.2f)));
            }
        }


        public bool IsPressed(MouseKey mouseKey)
        {
            if (MyKeyboard.IsPressed(mouseKey))
            {
                if (Hovered)
                {
                    if (Active)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
