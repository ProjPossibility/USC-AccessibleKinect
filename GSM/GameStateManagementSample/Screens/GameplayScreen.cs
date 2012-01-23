#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;

        Random random = new Random();

        Tank player1;
        Tank player2;

        bool isPlayer1Turn;

        SpriteFont gameFont;

        //Background environment
        Texture2D world;
        Texture2D mountain;
        Vector2 mountainPosition;
        BoundingSphere mountainBounding;

        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");

            ScreenManager.Game.ResetElapsedTime();

            player1 = new Tank(ScreenManager.Game, "Player1");
            ScreenManager.Game.Components.Add(player1);
            player1.tankState = Tank.TankState.Aiming;

            player1.isActive = true;

            player2 = new Tank(ScreenManager.Game, "Player2");
            ScreenManager.Game.Components.Add(player2);

            world = content.Load<Texture2D>("world");
            mountain = content.Load<Texture2D>("Mountain");
            mountainPosition = new Vector2(450, 475);

            mountainBounding = new BoundingSphere(new Vector3(mountainPosition.X + 200, 550, 0), (float)100);

            isPlayer1Turn = true;
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                if (player1.isActive == false && player2.isActive == false)
                {
                    if (isPlayer1Turn)
                    {
                        isPlayer1Turn = false;
                    }
                    else
                    {
                        //New Turn!
                        isPlayer1Turn = true;
                    }
                }

                //Player 1's Turn
                if (isPlayer1Turn)
                {
                    player1.isActive = true;
                    player2.isActive = false;
                    isPlayer1Turn = true;
                    if (player1.tankState != Tank.TankState.Firing)
                    {
                        player1.tankState = Tank.TankState.Aiming;
                        player2.tankState = Tank.TankState.Idle;
                    }
                   
                }
                //Player 2's Turn
                else
                {
                    player1.isActive = false;
                    player2.isActive = true;
                    isPlayer1Turn = false;
                    if (player2.tankState != Tank.TankState.Firing)
                    {
                        player1.tankState = Tank.TankState.Idle;
                        player2.tankState = Tank.TankState.Aiming;
                    }
                }

                Rectangle grassBounding = new Rectangle(-600,700,10000,50);
                //Collision
                if(player1.intersects(mountainBounding) && (player1.moveState == Tank.TankMovementState.MovingRight)) {
                    player1.worldPosition = new Vector2((mountainPosition.X - player1.texture.Width), player1.worldPosition.Y);
                }

                if (player2.intersects(mountainBounding) && (player2.moveState == Tank.TankMovementState.MovingLeft))
                {
                    player2.worldPosition = new Vector2((mountainPosition.X + mountain.Width), player2.worldPosition.Y);
                }

                if (player1.isActive && player1.tankState == Tank.TankState.Firing)
                {
                    if (player1.missile.intersects(mountainBounding))
                    {
                        //Remove the missile from the field
                        ScreenManager.Game.Components.Remove(player1.missile);
                        
                        //No points
                        //player1.isActive = !player1.isActive;
                        //player2.isActive = !player2.isActive;
                        isPlayer1Turn = false;
                        player1.missile = null;
                    }
                    else if (player1.missile.intersects(player2.boundingFrame))
                    {
                        ScreenManager.Game.Components.Remove(player1.missile);

                        player1.score += 5;
                        //player1.isActive = !player1.isActive;
                        //player2.isActive = !player2.isActive;
                        isPlayer1Turn = false;
                        player1.missile = null;
                    }
                    else if (player1.missile.intersects(grassBounding))
                    {
                        ScreenManager.Game.Components.Remove(player1.missile);
                        isPlayer1Turn = false;
                        player1.missile = null;
                    }
                }

                if(player2.isActive && player2.tankState == Tank.TankState.Firing)
                {
                    if (player2.missile.intersects(mountainBounding))
                    {
                        //Remove the missile
                        ScreenManager.Game.Components.Remove(player2.missile);
                        //player2.isActive = !player2.isActive;
                        //player1.isActive = !player1.isActive;
                        //No points
                        isPlayer1Turn = true;
                        player2.missile = null;
                    }
                    else if (player2.missile.intersects(player1.boundingFrame))
                    {
                        //Remove the missile 
                        ScreenManager.Game.Components.Remove(player2.missile);
                        //player1.isActive = !player1.isActive;
                        //player2.isActive = !player2.isActive;
                        isPlayer1Turn = true;
                        player2.missile = null;
                        //Add Points to player2
                        player2.score += 5;

                    }
                    else if (player2.missile.intersects(grassBounding))
                    {
                        ScreenManager.Game.Components.Remove(player2.missile);
                        isPlayer1Turn = true;
                        player2.missile = null;
                    }
                   
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                if (input.IsTankFire(ControllingPlayer)) {
                    if (isPlayer1Turn)
                    {
                        double missileAngle = ((player1.shotAngle /** (Math.PI / 180)*/));
                        Console.WriteLine("The missile angle is: "+missileAngle);
                        Projectile missile;
                        if (missileAngle < 0)
                        {
                           missile = new Projectile(ScreenManager.Game,
                           new Vector2(player1.cannonLocation.X - (float)(Math.Cos(missileAngle) * player1.cannonTexture.Height), (float)(Math.Sin(missileAngle) * player1.cannonTexture.Height) + player1.cannonLocation.Y - player1.texture.Height), new Vector2(-player1.force, player1.force));
                        }
                        else
                        {
                           missile = new Projectile(ScreenManager.Game,
                           new Vector2(player1.cannonLocation.X + (float)(Math.Cos(missileAngle) * player1.cannonTexture.Height), (float)(Math.Sin(missileAngle) * player1.cannonTexture.Height) + player1.cannonLocation.Y - player1.texture.Height), new Vector2(player1.force, player1.force));
                        }

                        player1.fireMissile(missile);
                        ScreenManager.Game.Components.Add(missile);
                    }
                    else
                    {
                        double missileAngle = ((player2.shotAngle /** (Math.PI / 180)*/));
                        Console.WriteLine("The missile angle is: " + missileAngle);
                        Projectile missile;
                        if (missileAngle < 0)
                        {
                            missile = new Projectile(ScreenManager.Game,
                            new Vector2(player2.cannonLocation.X - (float)(Math.Cos(missileAngle) * player2.cannonTexture.Height), (float)(Math.Sin(missileAngle) * player2.cannonTexture.Height) + player2.cannonLocation.Y - player2.texture.Height), new Vector2(player2.force, player2.force));
                        }
                        else
                        {
                            missile = new Projectile(ScreenManager.Game,
                            new Vector2(player2.cannonLocation.X + (float)(Math.Cos(missileAngle) * player2.cannonTexture.Height), (float)(Math.Sin(missileAngle) * player2.cannonTexture.Height) + player2.cannonLocation.Y - player2.texture.Height), new Vector2(-player2.force, player2.force));
                        } 
                        
                        player2.fireMissile(missile);
                        ScreenManager.Game.Components.Add(missile);
                    }
                }

                if (input.IsTankMovingLeft(ControllingPlayer))
                {
                    if (isPlayer1Turn)
                    {
                        player1.moveTank("Left");
                    }
                    else
                    {
                        player2.moveTank("Left");
                    }
                }

                if (input.IsTankMovingRight(ControllingPlayer))
                {
                    if (isPlayer1Turn)
                    {
                        player1.moveTank("Right");
                    }
                    else
                    {
                        player2.moveTank("Right");
                    }
                }

                if (input.IsAimUp(ControllingPlayer))
                {
                    if (isPlayer1Turn)
                    {
                        player1.changeAim("Up");
                    }
                    else
                    {
                        player2.changeAim("Up");
                    }
                }

                if (input.IsAimDown(ControllingPlayer))
                {
                    if (isPlayer1Turn)
                    {
                        player1.changeAim("Down");
                    }
                    else
                    {
                        player2.changeAim("Down");
                    }
                }

                if (input.IsAimChanged(ControllingPlayer) != -1)
                {
                    if (isPlayer1Turn)
                    {
                        player1.shotAngle = input.IsAimChanged(ControllingPlayer);
                    }
                    else
                    {
                        player2.shotAngle = input.IsAimChanged(ControllingPlayer);
                    }
                }
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            /*ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);*/

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Violet, 0, 0);
            DrawBackground(spriteBatch);
            DrawHUD(spriteBatch);
            player1.Draw(gameTime, spriteBatch);
            player2.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(world, new Rectangle(0, 700, 1260, 500), Color.White);
            spriteBatch.Draw(mountain, mountainPosition, Color.White);
        }

        public void DrawHUD(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(gameFont, "Player 1", new Vector2(100, 100), Color.Blue);
            spriteBatch.DrawString(gameFont, "Player 2", new Vector2(1000, 100), Color.Red);
            spriteBatch.DrawString(gameFont, "Angle: " + (int)(player1.shotAngle * (90 / 1.6)), new Vector2(100, 150), Color.Blue);
            spriteBatch.DrawString(gameFont, "Angle: " + (int)(player2.shotAngle * (90 / 1.6)), new Vector2(1000, 150), Color.Red);
            spriteBatch.DrawString(gameFont, "Force: " + player1.force.ToString(), new Vector2(100, 200), Color.Blue);
            spriteBatch.DrawString(gameFont, "Force: " + player2.force.ToString(), new Vector2(1000, 200), Color.Red);
            spriteBatch.DrawString(gameFont, "Score: " + player1.score.ToString(), new Vector2(100, 250), Color.Blue);
            spriteBatch.DrawString(gameFont, "Score: " + player2.score.ToString(), new Vector2(1000, 250), Color.Red);
            spriteBatch.DrawString(gameFont, "Status: " + player1.tankState.ToString(), new Vector2(100, 300), Color.Blue);
            spriteBatch.DrawString(gameFont, "Status: " + player2.tankState.ToString(), new Vector2(1000, 300), Color.Red);
        }

        #endregion
    }
}
