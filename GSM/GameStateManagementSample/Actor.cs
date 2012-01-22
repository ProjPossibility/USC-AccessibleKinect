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
    class Actor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Utils.Timer actorTimer;
        protected Vector2 worldPosition;
        protected String textureName;
        Texture2D texture;
        ContentManager contentManager;

        public Actor(Game game)
            : base(game)
        {
            actorTimer = new Utils.Timer();
            worldPosition = Vector2.Zero;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            float currentGameTime = ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000.0f);
            actorTimer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            contentManager = new ContentManager(Game.Services, "Content");
            texture = contentManager.Load<Texture2D>(textureName);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);

        }
    }
}
