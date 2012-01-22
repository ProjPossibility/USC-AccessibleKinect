#region File Description
//-----------------------------------------------------------------------------
// Projectile.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameStateManagement
{
    class Projectile : Actor
    {
        SpriteBatch spriteBatch;
        Game curGame;
        Random random;

        // Textures for projectile
        string textureName;
        // Position and speed of projectile
        Vector2 projectileVelocity = Vector2.Zero;
        float projectileInitialVelocityY;
        Vector2 projectileRotationPosition = Vector2.Zero;
        float projectileRotation;
        float flightTime;
        bool isAI;
        float hitOffset;
        float gravity;
        float myForceValue;

        Vector2 projectileStartPosition;
        public Vector2 ProjectileStartPosition
        {
            get
            {
                return projectileStartPosition;
            }
            set
            {
                projectileStartPosition = value;
            }
        }

        //Vector2 projectilePosition = Vector2.Zero;
        /*public Vector2 ProjectilePosition
        {
            get
            {
                return projectilePosition;
            }
            set
            {
                projectilePosition = value;
            }
        }*/

        // Gets the position where the projectile hit the ground.
        // Only valid after a hit occurs.
        public Vector2 ProjectileHitPosition { get; private set; }

        Texture2D projectileTexture;
        public Texture2D ProjectileTexture
        {
            get
            {
                return projectileTexture;
            }
            set
            {
                projectileTexture = value;
            }
        }

        public Projectile(Game game)
            : base(game)
        {
            curGame = game;
            random = new Random();
        }

        public Projectile(Game game,Vector2 startPosition, float forceValue)
            : this(game)
        {

            worldPosition = startPosition;
            textureName = "Missile";
      
            myForceValue = forceValue;
            Fire(startPosition.X * forceValue, startPosition.Y * forceValue);
    
        }

        public override void Initialize()
        {
            // Load a projectile texture
            projectileTexture = curGame.Content.Load<Texture2D>(textureName);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(projectileTexture, this.worldPosition, null,
                Color.White, projectileRotation,
                new Vector2(projectileTexture.Width / 2,
                            projectileTexture.Height / 2),
                1.0f, SpriteEffects.None, 0);

            base.Draw(gameTime);
        }

        public void UpdateProjectileFlightData(GameTime gameTime, float wind, float gravity, out bool groundHit)
        {
            flightTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate new projectile position using standard
            // formulas, taking the wind as a force.
            //int direction = isAI ? -1 : 1;

            float previousXPosition = worldPosition.X;
            float previousYPosition = worldPosition.Y;

            worldPosition.X = projectileStartPosition.X +
                (/*direction*/ projectileVelocity.X * flightTime) +
                0.5f * (8 /* wind */ * (float)Math.Pow(flightTime, 2));

            worldPosition.Y = projectileStartPosition.Y - (projectileVelocity.Y * flightTime) + (float)(0.5 * (-9.8 * Math.Pow(flightTime, 2)));

            // Calculate the projectile rotation
            //projectileRotation += MathHelper.ToRadians(projectileVelocity.X * 0.5f);

            // Check if projectile hit the ground or even passed it 
            // (could happen during normal calculation)
            if (worldPosition.Y >= 332 + hitOffset)
            {
                worldPosition.X = previousXPosition;
                worldPosition.Y = previousYPosition;

                ProjectileHitPosition = new Vector2(previousXPosition, 332);

                groundHit = true;
            }
            else
            {
                groundHit = false;
            }
        }

        public void Fire(float velocityX, float velocityY)
        {
            // Set initial projectile velocity
            projectileVelocity.X = velocityX;
            projectileVelocity.Y = velocityY;
            projectileInitialVelocityY = projectileVelocity.Y;
            // Reset calculation variables
            flightTime = 0;
        }
    }
}
