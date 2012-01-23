using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameStateManagement
{
    class Tank : Actor
    {
        //Cannon related variables
        public float shotAngle;
        public float force;
        public Vector2 cannonLocation;
        public Texture2D cannonTexture;
        public Projectile missile;

        public int score;
        public bool isActive;
        public enum TankState
        {
            Idle,
            Aiming,
            Reset,
            Firing
        }

        public enum TankMovementState
        {
            Idle,
            MovingLeft,
            MovingRight
        }

        public TankState tankState;
        public TankMovementState moveState;

        bool myBHuman;
        public bool bHuman
        {
            get
            {
                return myBHuman;
            }
            set
            {
                myBHuman = value;
            }
        }

        public Tank(Game game, string playerType)
            : base(game)
        {
            if (playerType == "Player1")
            {
                worldPosition = new Vector2(100, 600);
            }
            else
            {
                worldPosition = new Vector2(1000, 600);
            }

            isActive = false;
        }

        public override void Initialize()
        {
            textureName = "Tank";
            tankState = TankState.Idle;
            moveState = TankMovementState.Idle;
            score = 0;

            //Set-up Cannon
            shotAngle = 0;
            force = 50;
            contentManager = new ContentManager(Game.Services, "Content");
            cannonTexture = contentManager.Load<Texture2D>("Cannon");
            cannonLocation = new Vector2((worldPosition.X + 50), (worldPosition.Y + 10));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            cannonLocation = new Vector2((worldPosition.X + 50), (worldPosition.Y + 10));
            base.Update(gameTime);
        }

        public void endCannonShot()
        {
            score++;
            isActive = false;
        }

        public void fireMissile(Projectile newMissile)
        {
            if (tankState != TankState.Idle || tankState != TankState.Firing)
            {
                //You fired a projectile
                missile = newMissile;
                tankState = TankState.Firing;
            }
        }

        public void moveTank(String direction)
        {
            if (tankState != TankState.Idle || tankState != TankState.Firing)
            {
                if (direction == "Left")
                {
                    worldPosition.X -= 10;
                    moveState = TankMovementState.MovingLeft;
                }
                else
                {
                    worldPosition.X += 10;
                    moveState = TankMovementState.MovingRight;
                }
            }
        }

        public void changeAim(String angleDirection)
        {
            if (tankState != TankState.Idle || tankState != TankState.Firing)
            {
                if (angleDirection == "Down" && (shotAngle >= -Math.PI/2))//-1.6))
                {
                    shotAngle -= (float).1;
                }
                else if(angleDirection == "Up" && (shotAngle <= Math.PI/2))//1.6))
                {
                    shotAngle += (float).1;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, worldPosition, Color.White);
            spriteBatch.Draw(cannonTexture, cannonLocation, null, Color.White, shotAngle, new Vector2(10, 60), 1, SpriteEffects.FlipVertically, 1);
            //projectile.Draw(gameTime, spriteBatch);
            if (missile != null)
            {
                if (missile.texture != null)
                {
                    spriteBatch.Draw(missile.texture, missile.worldPosition, null,
                    Color.White, missile.projectileRotation,
                    new Vector2(missile.texture.Width / 2,
                            missile.texture.Height / 2),
                    1.0f, SpriteEffects.None, 0);
                }
            }
            base.Draw(gameTime);
        }
    }
}
