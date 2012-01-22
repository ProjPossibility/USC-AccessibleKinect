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

            ScreenManager.Game.ResetElapsedTime();

            player1 = new Tank(ScreenManager.Game);
            ScreenManager.Game.Components.Add(player1);
            player1.tankState = Tank.TankState.Aiming;


            player2 = new Tank(ScreenManager.Game);
            ScreenManager.Game.Components.Add(player2);

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
                if ((player1.tankState == Tank.TankState.Reset ||
                    player2.tankState == Tank.TankState.Reset) &&
                       !(player1.AnimationRunning ||
                        player2.AnimationRunning))
                {
                    //Player 1's Turn just finished
                    if (player1.isActive)
                    {
                        player1.isActive = false;
                        player2.isActive = true;
                        isPlayer1Turn = false;
                        player1.tankState = Tank.TankState.Idle;
                        player2.tankState = Tank.TankState.Aiming;
                    }
                    //Player 2's Turn just finished
                    else
                    {
                        player1.isActive = true;
                        player2.isActive = false;
                        isPlayer1Turn = true;
                        player1.tankState = Tank.TankState.Aiming;
                        player2.tankState = Tank.TankState.Idle;
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
                        player1.fireMissile();
                    }
                    else
                    {
                        player2.fireMissile();
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

            spriteBatch.Draw(player1.texture, player1.worldPosition, Color.White);
            spriteBatch.Draw(player2.texture, player2.worldPosition, Color.White);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
