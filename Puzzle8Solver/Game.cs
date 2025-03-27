using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Puzzle8Solver;

public class Game : Microsoft.Xna.Framework.Game
{
    public static GraphicsDeviceManager Graphics;
    public static SpriteBatch SpriteBatch;
    public static Game Self;

    public static SpriteFont Font;
    public static Button[] Buttons;

    private const int ButtonWidth = 240;
    private const int ButtonHeight = 60;
    private const int ButtonSpacing = 20;

    public Game()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Self = this;
    }

    protected override void Initialize()
    {
        base.Initialize();

        Graphics.PreferredBackBufferWidth = 1280;
        Graphics.PreferredBackBufferHeight = 720;
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Textures.Load(Content);

        Font = Content.Load<SpriteFont>("DefaultFont");

        Buttons = new Button[3] { new Button(new Rectangle(ButtonSpacing, ButtonSpacing, ButtonWidth, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "Solve"), new Button(new Rectangle(ButtonSpacing, ButtonSpacing * 2 + ButtonHeight, ButtonWidth, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "Generate"), new Button(new Rectangle(ButtonSpacing, ButtonSpacing * 3 + ButtonHeight * 2, ButtonWidth, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "") };
        Buttons[2].Active = false;
    }

    protected override void Update(GameTime gameTime)
    {
        float elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        MyKeyboard.Update();

        foreach (Button button in Buttons)
        {
            button.Update(elapsedTime);
        }

        PuzzleManager.Update(elapsedTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        SpriteBatch.Begin();

        foreach (Button button in Buttons)
        {
            button.Draw();
        }

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
