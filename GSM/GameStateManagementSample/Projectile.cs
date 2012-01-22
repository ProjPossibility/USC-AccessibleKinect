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
        Random random;

        // Textures for projectile
        // Position and speed of projectile
        Vector2 projectileVelocity = Vector2.Zero;
        float projectileInitialVelocityY;
        Vector2 projectileRotationPosition = Vector2.Zero;
        public float projectileRotation;
        float flightTime;
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

        // Gets the position where the projectile hit the ground.
        // Only valid after a hit occurs.
        public Vector2 ProjectileHitPosition { get; private set; }

        public Projectile(Game game)
            : base(game)
        {
            curGame = game;
            random = new Random();
        }

        public Projectile(Game game,Vector2 startPosition, float forceValue)
            : this(game)
        {
            curGame = game;

            worldPosition = startPosition;

            projectileStartPosition = startPosition;
            textureName = "Missile";
      
            myForceValue = forceValue;
            Fire(forceValue, forceValue);
        }

        public override void Initialize()
        {
            // Load a projectile texture
            //texture = curGame.Content.Load<Texture2D>(textureName);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateProjectileFlightData(gameTime); 
            base.Update(gameTime);
        }

        public void UpdateProjectileFlightData(GameTime gameTime)
        {
            flightTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            float previousXPosition = worldPosition.X;
            float previousYPosition = worldPosition.Y;

            worldPosition.X = projectileStartPosition.X +
                ( projectileVelocity.X * flightTime) +
                0.5f * (8  * (float)Math.Pow(flightTime, 2));

            worldPosition.Y = projectileStartPosition.Y - (projectileVelocity.Y * flightTime) + (float)(0.5 * (-9.8 * Math.Pow(flightTime, 2)));
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
