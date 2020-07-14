using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using SummerProject.Input;

namespace SummerProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameEngine : Game
    {
        // Reference to graphics has to be maintained here otherwise application can't draw anymore.    

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _playerSprite;
        private Texture2D _floorSprite;
        private Texture2D _enemySprite;
        private Player _player = new Player(new KeyboardKeys());
        private Enemy _enemy = new Enemy();
        private RenderTarget2D _nativeRenderTarget;
        private MapDetails _mapDetails;
        private SpriteFont _font;
        private int _score = 0;

        private readonly Location _screenSize = new Location
        {
            X = 480,
            Y = 240
        };
        
        private readonly Location _screenResolution = new Location
        {
            X = 800,
            Y = 480
        };

        public GameEngine()
        {
            // These also need to be left to set up the content directory and to initialise the graphics device even if they are not referenced here.
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 

        protected override void Initialize()
        {
            base.Initialize();
            _mapDetails = new MapDetails(new Location { X = 0, Y = 0 });
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _playerSprite = Content.Load<Texture2D>(_player.SpriteLocation);
            _enemySprite = Content.Load<Texture2D>(_enemy.SpriteLocation);
            _floorSprite = Content.Load<Texture2D>("Test_graphics/tile_1");
            _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, _screenSize.X, _screenSize.Y);
            _font = Content.Load<SpriteFont>("Font");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _player.GetPlayerInput();
            _mapDetails.CalculateMapPosition(_player.Location);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            var offset = _mapDetails.GetDrawOffset();
            var blockSize = 16;
            for (int y = 0; y < 30; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    _spriteBatch.Draw(
                        _floorSprite,
                        new Rectangle(
                            (x * blockSize) + offset.X,
                            (y * blockSize) + offset.Y,
                            blockSize,
                            blockSize),
                        Color.Crimson
                    );
                }
            }
            var playerDrawLocation = GetPlayerDrawLocation(_screenSize, _player);
            
            _spriteBatch.Draw(_playerSprite,
                new Rectangle(
                    playerDrawLocation.X,
                    playerDrawLocation.Y,
                    _player.Width,
                    _player.Height),
                Color.White
            );

            _spriteBatch.Draw(_enemySprite, 
                new Rectangle(
                    _enemy.X + offset.X,
                    _enemy.Y + offset.Y,
                    _enemy.Width,
                    _enemy.Height),
                Color.White
               );

            //drawing text
            _spriteBatch.DrawString(
                _font,
                $"Score: {_score}",
                new Vector2(10, 5),
                Color.AntiqueWhite
                );

            _spriteBatch.End();
            // now render your game like you normally would, but if you change the render target somewhere,
            // make sure you set it back to this one and not the backbuffer
            // after drawing the game at native resolution we can render _nativeRenderTarget to the backbuffer!
            // First set the GraphicsDevice target back to the backbuffer
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(samplerState: Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp);
            
            _spriteBatch.Draw(_nativeRenderTarget, new Rectangle(0, 0, _screenResolution.X, _screenResolution.Y), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Location GetPlayerDrawLocation(Location screenSize, Player player)
        {
            return new Location {
                X = GetCentralDrawPosition(screenSize.X, player.Width),
                Y = GetCentralDrawPosition(screenSize.Y, player.Height)
            };
        }


        private int GetCentralDrawPosition(int screenPoint, int playerPoint)
        {
            return (screenPoint / 2) - (playerPoint / 2);
        }
    }
}
