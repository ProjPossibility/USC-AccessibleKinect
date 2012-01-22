using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Utils
{
    public class FrameRateCounter : DrawableGameComponent
    {
        private ContentManager m_kContent;
        private SpriteBatch m_kSpriteBatch;
        private SpriteFont m_kFont;

        private Vector2 m_vPosition;

		private float m_fCurrentFrameRate;
        private float m_fAverageFrameRate;
        private float minimumFrameRate;
        private float maximumFrameRate;
        private float sumFrames;

        private const int framesToTrack = 100;
        private Queue<float> frameQueue = new Queue<float>();
        
        public FrameRateCounter(Game game, Vector2 vPosition)
            : base(game)
        {
            m_kContent = new ContentManager(game.Services);
            m_kContent.RootDirectory = "Content";
            minimumFrameRate = 5000;
            maximumFrameRate = 0;
            DrawOrder = 1000;

            m_vPosition = vPosition;
        }

        protected override void LoadContent()
        {
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService));

            m_kSpriteBatch = new SpriteBatch(graphicsService.GraphicsDevice);
            m_kFont = m_kContent.Load<SpriteFont>("fpsfont");
        }
        
        protected override void UnloadContent()
        {
            m_kContent.Unload();
        }
        
        public override void Update(GameTime gameTime)
        {
            sumFrames = 0;
			m_fCurrentFrameRate = 1 / ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000.0f);
			//We can't use the below because our framerate can be SO HIGH that the ms value rounds to zero
			//m_fCurrentFrameRate = 1 / ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            /*if (m_fCurrentFrameRate < 5)
            {
                m_fCurrentFrameRate = 50;
            }*/
          
            frameQueue.Enqueue(m_fCurrentFrameRate);

            while( frameQueue.Count > framesToTrack)
            {
                frameQueue.Dequeue();
            }

            foreach (int i in frameQueue)
            {
                if (i < minimumFrameRate && i > 0)
                {
                   minimumFrameRate = i;
                }
                if (i > maximumFrameRate)
                {
                    maximumFrameRate = i;
                }
                sumFrames += i;
            }

            m_fAverageFrameRate = sumFrames / frameQueue.Count;

			base.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            m_kSpriteBatch.Begin();
            
			// Color this based on the framerate
            Color DrawColor = Color.Green;
			if (m_fCurrentFrameRate < 15.0f)
                DrawColor = Color.Red;
			else if (m_fCurrentFrameRate < 30.0f)
                DrawColor = Color.Yellow;

			m_kSpriteBatch.DrawString(m_kFont, "Average FPS: " + m_fAverageFrameRate, m_vPosition, DrawColor);
            m_kSpriteBatch.DrawString(m_kFont, "Minimum FPS: " + minimumFrameRate, m_vPosition + new Vector2(0, 50), DrawColor);
            m_kSpriteBatch.DrawString(m_kFont, "Maximum FPS: " + maximumFrameRate, m_vPosition + new Vector2(0, 100), DrawColor);
            m_kSpriteBatch.End();

        }

		public void ResetFPSCount()
		{
            minimumFrameRate = 5000;
            maximumFrameRate = frameQueue.Count;
            for (int i = 0; i < maximumFrameRate; i++)
            {
                frameQueue.Dequeue();
            }
            maximumFrameRate = 0;
		}
    }
}
