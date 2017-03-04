using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HW2
{
    public class Game1 : Game
    {
        // default game attributes
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // enumeration
        enum GameStates {Menu, Game, GameOver};

        // attributes
        Texture2D playerSprites;
        Texture2D itemSprites;
        SpriteFont font;
        Player player;
        List<Collectible> collectibles;
        int level;
        double timer;
        KeyboardState kbState;
        KeyboardState previousKbState;
        Random rando = new Random();
        Rectangle largeWords;

        GameStates gameState; // holds current game state

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // changes window size
            graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;  // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        // initializes the game
        protected override void Initialize()
        {
            // add initilize logic
            player = new Player(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height, 60, 60); 
            collectibles = new List<Collectible>(); 
            gameState = GameStates.Menu; // sets gamestate to menu for the start of the game

            // places the menu title and game over
            largeWords = new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 150, 50);

            base.Initialize();
        }

        // loads content
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerSprites = this.Content.Load<Texture2D>("player_sprites");
            itemSprites = this.Content.Load<Texture2D>("item_sprites");
            font = this.Content.Load<SpriteFont>("SpriteFont1");

            player.CurTexture = playerSprites;

        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // updates the game at a predetermined interval
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // will exit game if "Escape" button is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // begin Logic
            // checks to determine what game state is currently happening
            if(gameState == GameStates.Menu) 
            {
                // save keyboard states
                previousKbState = kbState;
                kbState = Keyboard.GetState();

                // check for a single enter key press
                if (SingleKeyPress(Keys.Enter) == true)
                {
                    ResetGame();
                    gameState = GameStates.Game;
                }
            }

            if (gameState == GameStates.GameOver) 
            {
                // save keyboard states
                previousKbState = kbState;
                kbState = Keyboard.GetState();

                // check for a single enter key press
                if (SingleKeyPress(Keys.Enter) == true)
                {
                    gameState = GameStates.Menu;
                }
            }

            if (gameState == GameStates.Game)
            {
                // save keyboard states
                previousKbState = kbState;
                kbState = Keyboard.GetState();

                // decrement timer
                timer = timer - gameTime.ElapsedGameTime.TotalSeconds;

                // movement controls
                ProcessInput();
                ScreenWrap(player);

                // verify collectibles
                for(int x = 0; x < collectibles.Count; x++)
                {
                    if(collectibles[x].CheckCollision(player))
                    {
                        collectibles[x].Active = false;
                        player.LevelScore += 1;
                        player.TotalScore += 1;
                    }
                }

                // check if timer has dropped to zero
                if(timer < 0)
                {
                    gameState = GameStates.GameOver;
                }

                // check if all collectibles have been found
                if(player.LevelScore == collectibles.Count)
                {
                    NextLevel();
                }
            }



            // end Logic
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // starts spriteBatch
            spriteBatch.Begin();

            // drawing code
            if (gameState == GameStates.Menu) // draws menu features
            {
                spriteBatch.DrawString(font, "Treasure Hunter", new Vector2(225, 100), Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Press ENTER to begin", new Vector2(325, 200), Color.White);
            }

            if (gameState == GameStates.Game) // draws game features
            {
                // draw player
                spriteBatch.Draw(player.CurTexture, player.Location, new Rectangle(0,0,30,30), Color.White);

                // draw collectibles
                for (int x = 0; x < collectibles.Count; x++)
                {
                    collectibles[x].Draw(spriteBatch);
                    //spriteBatch.Draw(collectibles[x].CurTexture, collectibles[x].Location, new Rectangle(0, 90, 30, 30), Color.White);
                    //spriteBatch.Draw(player.CurTexture, collectibles[x].Location, new Rectangle(0, 0, 30, 30), Color.White);
                }

                // words
                spriteBatch.DrawString(font, "Level: " + level, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(font, "Score: " + player.LevelScore, new Vector2(100, 0), Color.White);
                spriteBatch.DrawString(font, "Time: " + (string.Format("{0:0.00}", timer)), new Vector2(200, 0), Color.White);
            }

            if (gameState == GameStates.GameOver) // draws gameover features
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(225, 100), Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Level: " + level, new Vector2(325, 200), Color.White);
                spriteBatch.DrawString(font, "Score: " + player.TotalScore, new Vector2(325, 250), Color.White);
                spriteBatch.DrawString(font, "Press ENTER to begin", new Vector2(325, 300), Color.White);
            }

            // closes spriteBatch
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // additional methods

        // sets up the next level for the player
        public void NextLevel()
        {
            // increment level number
            level += 1;

            // set timer to starting value
            timer = 10.0;

            // reset level score property
            player.LevelScore = 0;

            // center player on screen
            player.XLocation = GraphicsDevice.Viewport.Width / 2;
            player.YLocation = GraphicsDevice.Viewport.Height / 2;

            // clear list of collectibles
            collectibles.Clear();

            // calculate the amount of collectibles
            int collectAmnt = 2 + (level * 2);

            // generate collectibles
            for(int x = 0; x < collectAmnt; x++)
            {
                // variables
                int xPos;
                int yPos;
                int widHei;

                // generate random numbers for position
                xPos = rando.Next(0, GraphicsDevice.Viewport.Width - 30);
                yPos = rando.Next(0, GraphicsDevice.Viewport.Height - 30);

                // set collectible size
                widHei = 60;
                             
                // add collectible to List<>  // set texture of collectible
                collectibles.Add(new Collectible(xPos, yPos, widHei, widHei));
                collectibles[x].CurTexture = itemSprites;
            }
        }

        // sets up the game when transitioning from menu to game state
        public void ResetGame()
        {
            level = 0;

            player.TotalScore = 0;

            NextLevel();
        }

        // keeps the player on screen at all times by sending it to the opposite side of the screen
        public void ScreenWrap(GameObject obj)
        {
            // variables
            int xMax = GraphicsDevice.Viewport.Width;
            int yMax = GraphicsDevice.Viewport.Height;

            // wraps from right to left
            if(player.XLocation >= xMax + 35)
            {
                player.XLocation = -25;
            }
            
            // wraps from left to right
            if(player.XLocation <= 0 - 35)
            {
                player.XLocation = xMax + 25;
            }


            // wraps from bottom to top 
            if (player.YLocation >= yMax + 35)
            {
                player.YLocation = -25;
            }

            // wraps from top to bottom
            if (player.YLocation <= 0 - 35)
            {
                player.YLocation = yMax + 25;
            }
        }

        // 
        public bool SingleKeyPress(Keys keyValue)
        {
            // variables
            bool pressed = false;

            // check against the last two keyboard states to determine if this is the first time key has been pressed
            if(kbState.IsKeyDown(keyValue) && previousKbState.IsKeyUp(keyValue))
            {
                pressed = true;
            }
            return pressed;
        }

        // controls movement
        public void ProcessInput()
        {
            // get and store value of the keyboard
            KeyboardState keyState = Keyboard.GetState();

            // determine if W, A, S or D buttons are pressed down respectively
            if (keyState.IsKeyDown(Keys.W))
            {
                player.YLocation -= 5;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                player.XLocation -= 5;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                player.YLocation += 5;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                player.XLocation += 5;
            }

        }
    }
}
