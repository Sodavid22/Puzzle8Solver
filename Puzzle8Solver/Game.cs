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

    public const int ButtonWidth = 240;
    public const int ButtonHeight = 60;
    public const int ButtonSpacing = 20;

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

        Buttons = new Button[5] { new Button(new Rectangle(ButtonSpacing, ButtonSpacing, ButtonWidth, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "Solve")
            , new Button(new Rectangle(ButtonSpacing, ButtonSpacing * 2 + ButtonHeight, ButtonWidth, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "Generate")
            , new Button(new Rectangle(ButtonSpacing, ButtonSpacing * 3 + ButtonHeight * 2, ButtonWidth, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "Clear")
            , new Button(new Rectangle(ButtonSpacing, ButtonSpacing * 4 + ButtonHeight * 3, ButtonWidth/2, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "Previous")
            , new Button(new Rectangle(ButtonSpacing + ButtonWidth/2, ButtonSpacing * 4 + ButtonHeight * 3, ButtonWidth/2, ButtonHeight), new Vector4(0, 1, 0, 1), 2, "Next") };

        PuzzleManager.Load();
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

        if (Buttons[0].IsPressed(MouseKey.Left) && PuzzleManager.RemainingNumbers.Count == 0) // solve
        {
            PuzzleManager.Solve();
        }

        if (Buttons[1].IsPressed(MouseKey.Left)) // generate
        {
            PuzzleManager.GenerateMatrix();
        }

        if (Buttons[2].IsPressed(MouseKey.Left)) // clear
        {
            PuzzleManager.ClearInput();
        }

        if (Buttons[3].IsPressed(MouseKey.Left) && PuzzleManager.CurrentStep < PuzzleManager.FinalSteps.Count - 1) // previous
        {
            PuzzleManager.CurrentStep++;
        }

        if (Buttons[4].IsPressed(MouseKey.Left) && PuzzleManager.CurrentStep > 0) // next
        {
            PuzzleManager.CurrentStep--;
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

        PuzzleManager.Draw();

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
