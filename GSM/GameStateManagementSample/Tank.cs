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
        float shotAngle;
        public Vector2 cannonLocation;
        public Texture2D cannonTexture;
        public Projectile cannonShot;

        public bool isActive;
        public enum TankState
        {
            Idle,
            Aiming,
            Reset
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

            //Set-up Cannon
            shotAngle = 0;
            contentManager = new ContentManager(Game.Services, "Content");
            cannonTexture = contentManager.Load<Texture2D>("Cannon");
            cannonLocation = new Vector2((worldPosition.X + 110), (worldPosition.Y + 10));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            cannonLocation = new Vector2((worldPosition.X + 110), (worldPosition.Y + 10));
            base.Update(gameTime);
        }

        public void endCannonShot()
        {
            isActive = false;
        }

        public void fireMissile()
        {
            if (tankState != TankState.Idle)
            {
                //You fired a projectile!
                cannonShot = new Projectile();

                actorTimer = new Utils.Timer();
                actorTimer.AddTimer("Fake Projectile", 4.0f, endCannonShot, false);
                tankState = TankState.Idle;
            }
        }

        public void moveTank(String direction)
        {
            if (tankState != TankState.Idle)
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
            if (tankState != TankState.Idle)
            {
                if (angleDirection == "Down")
                {
                    shotAngle -= (float)0.1;
                }
                else
                {
                    shotAngle += (float)0.1;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, worldPosition, Color.White);
            spriteBatch.Draw(cannonTexture, cannonLocation, null, Color.White, shotAngle, new Vector2(10, 60), 1, SpriteEffects.FlipVertically, 1);
            //projectile.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
