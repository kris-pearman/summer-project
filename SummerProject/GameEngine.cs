﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using SummerProject.Input;
using System;
using System.Collections.Generic;

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
        private Texture2D _wallSprite;
        private Texture2D _ceilingSprite;
        private Player _player = new Player(new KeyboardKeys());
        private Enemy _enemy = new Enemy();
        private RenderTarget2D _nativeRenderTarget;
        private MapDetails _mapDetails;
        private SpriteFont _font;
        private int _score = 0;
        List<Enemy> _enemyList = new List<Enemy>();

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
            populate_enemy_list();
        }

        private void populate_enemy_list()
        {
            //Making a list of 3 enemies in different positions just so I can see how it's done later on
            _enemyList.Add(new Enemy() { X = 30, Y = 30 });
            _enemyList.Add(new Enemy() { X = 50, Y = 50 });
            _enemyList.Add(new Enemy() { X = 80, Y = 70 });
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
            _wallSprite = Content.Load<Texture2D>("Test_graphics/wall_lowmid");
            _ceilingSprite = Content.Load<Texture2D>("Test_graphics/wall_mid");
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
            DrawBackground(offset);
            var playerDrawLocation = GetPlayerDrawLocation(_screenSize, _player);

            DrawWalls(offset);
            DrawEnemies(offset);
            DrawPlayer(playerDrawLocation);
 

            _spriteBatch.End();

            //draws everything previously stated to the actual screen size as opposed to a smaller resolution
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(samplerState: Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp);
            _spriteBatch.Draw(_nativeRenderTarget, new Rectangle(0, 0, _screenResolution.X, _screenResolution.Y), Color.White);

            //Anything drawn after here will not be rescaled to the window size, probably best for text?
            //drawing text
            DrawScore();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private void DrawWalls(Location offset)
        {
            for (int x = 0; x<50; x++)
            {
                _spriteBatch.Draw(
                    _wallSprite,
                    new Rectangle(
                        x * 32 + offset.X,
                        0 + offset.Y,
                        32,
                        32),
                    Color.White
                    );
                                _spriteBatch.Draw(
                    _ceilingSprite,
                    new Rectangle(
                        x * 32 + offset.X,
                        -32 + offset.Y,
                        32,
                        32),
                    Color.White
                    );
            }
        }
        private void DrawBackground(Location offset)
        {
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
        }

        private void DrawScore()
        {
            _spriteBatch.DrawString(
                            _font,
                            $"Score: {_score}",
                            new Vector2(10, 5),
                            Color.AntiqueWhite
                            );
        }

        private void DrawPlayer(Location playerDrawLocation)
        {
            _spriteBatch.Draw(_playerSprite,
            new Rectangle(
                playerDrawLocation.X,
                playerDrawLocation.Y,
                _player.Width,
                _player.Height),
            Color.White
            );
        }

        private void DrawEnemies(Location offset)
        {
            foreach (Enemy _enemy in _enemyList)
            {
                _spriteBatch.Draw(_enemySprite,
                new Rectangle(
                    _enemy.X + offset.X,
                    _enemy.Y + offset.Y,
                    _enemy.Width,
                    _enemy.Height),
                Color.White
               );
            }
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
