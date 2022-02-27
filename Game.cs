using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace NeonShooter
{
    public class NeonShooterGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static NeonShooterGame Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }
        bool paused = false;
        public NeonShooterGame()
        {
            Instance = this; // used for fetching the game viewport
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // TODO: Add your initialization logic here
            EntityManager.Add(PlayerShip.Instance);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Sound.Music);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Art.Load(Content);
            Sound.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime; // allow other methods to access the GameTime property
            // TODO: Add your update logic here
            Input.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P))
                paused = !paused;

            if (!paused)
            {
                PlayerStatus.Update();
                EntityManager.Update();
                EnemySpawner.Update();

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(_spriteBatch);

            _spriteBatch.DrawString(Art.UiFont, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
            DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
            DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);
            // draw the custom mouse cursor
            _spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            if (PlayerStatus.IsGameOver)
            {
                string text = "Game Over\n" +
                    "Your Score: " + PlayerStatus.Score + "\n" +
                    "High Score: " + PlayerStatus.HighScore;

                Vector2 textSize = Art.UiFont.MeasureString(text);
                _spriteBatch.DrawString(Art.UiFont, text, ScreenSize / 2 - textSize / 2, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.UiFont.MeasureString(text).X;
            _spriteBatch.DrawString(Art.UiFont, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
    }
}
