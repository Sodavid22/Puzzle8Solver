using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Puzzle8Solver
{
    public static class Textures
    {
        public static Texture2D EmptyTexture { get; private set; }

        public static void Load(ContentManager contentManager)
        {
            Textures.EmptyTexture = new Texture2D(Game.Graphics.GraphicsDevice, 1, 1);
            Textures.EmptyTexture.SetData(new[] { Color.White });
        }
    }
}
