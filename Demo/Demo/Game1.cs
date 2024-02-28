using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace Demo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _texture;
        private SpriteFont _font;
        private Rectangle _rectangle;
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private int _score = 0;
        private int _totalCoins = 0;
        private MouseState _previousMouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _texture = Content.Load<Texture2D>("coin");
            _font = Content.Load<SpriteFont>("Font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouseState = Mouse.GetState();
            if (SingleClickCheck())
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (_totalCoins < 10)
                    {
                        _rectangle = new Rectangle(mouseState.X - _texture.Width / 2, mouseState.Y - _texture.Height / 2, _texture.Width, _texture.Height);
                        _rectangles.Add(_rectangle);
                        _totalCoins++;
                    }
                    else
                    {
                        _rectangle = new Rectangle(mouseState.X - _texture.Width / 2, mouseState.Y - _texture.Height / 2, _texture.Width, _texture.Height);
                        _rectangles.Add(_rectangle);
                        for (int i = 0; i < _rectangles.Count; i++)
                        {
                            if (_rectangles.Remove(_rectangles[i]))
                            {

                                break;
                            }
                        }
                    }
                }
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    for (int i = 0; i < _rectangles.Count; i++)
                    {
                        if (IsMouseOver(_rectangles[i]))
                        {
                            _rectangles.RemoveAt(i);
                            _totalCoins--;
                        }
                    }
                }
            }

            


            // TODO: Add your update logic here
            _previousMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (var rectangle in _rectangles)
            {
                if (IsMouseOver(rectangle))
                {
                    _spriteBatch.Draw(_texture, rectangle, Color.Red);
                }
                else
                {
                    _spriteBatch.Draw(_texture, rectangle, Color.White);
                }
            }
            _spriteBatch.DrawString(_font, "Number of coins: " + _rectangles.Count, new Vector2(10, 10), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public bool IsMouseOver(Rectangle rectangle)
        {
            var mouseState = Mouse.GetState();
            var mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            return mouseRectangle.Intersects(rectangle);
        }

        public bool SingleClickCheck()
        {
            var mouseState = Mouse.GetState();
            if ((_previousMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                || (_previousMouseState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed))
            {
                return true;
            }
            return false;
        }
    }
}
