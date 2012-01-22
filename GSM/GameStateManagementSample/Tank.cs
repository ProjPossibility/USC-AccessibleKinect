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
        float shotAngle;
        public bool isActive;
        public bool AnimationRunning;
        public enum TankState
        {
            Idle,
            Aiming,
            Reset
        }

        public TankState tankState;

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
        public Tank(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            textureName = "Tank";
            worldPosition = new Vector2(100, 100);
            tankState = TankState.Idle;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void fireMissile()
        {

        }

        public void moveTank(String direction)
        {
            if (direction == "Left")
            {
                worldPosition.X -= 10;
            }
            else
            {
                worldPosition.X += 10;
            }
        }

        public void changeAim(String angleDirection)
        {
            if (angleDirection == "Down")
            {
                shotAngle -= 1;
            }
            else
            {
                shotAngle += 1;
            }
        }
    }
}
