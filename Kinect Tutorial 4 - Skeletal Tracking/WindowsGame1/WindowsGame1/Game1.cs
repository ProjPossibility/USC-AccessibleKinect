using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Research.Kinect.Nui;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D handCursor;

        Vector2 rightHandPosition = new Vector2(0.0f);
        Vector3 leftHand = Vector3.Zero;
        Vector3 rightHand = Vector3.Zero;
        Vector3 head = Vector3.Zero;

        //NUICursor cursorHand,cursorElbow;


        bool useMouse = false;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //cursorHand = new NUICursor(this);
            //cursorElbow = new NUICursor(this);
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //cursorHand.Initialize(1.0f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, true);
            //cursorElbow.Initialize(1.0f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            handCursor = Content.Load<Texture2D>("hand");

            //cursorHand.CreateEvent();
            //cursorElbow.CreateEvent();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void OnSkeletonFrameReady(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            Microsoft.Research.Kinect.Nui.SkeletonFrame skeletonFrame = e.SkeletonFrame;

            foreach (Microsoft.Research.Kinect.Nui.SkeletonData data in skeletonFrame.Skeletons)
            {
                if (data.TrackingState == Microsoft.Research.Kinect.Nui.SkeletonTrackingState.Tracked)
                {
                    Vector3 leftHand = Vector3.Zero;
                    Vector3 rightHand = Vector3.Zero;
                    Vector3 head = Vector3.Zero;

                    foreach (Microsoft.Research.Kinect.Nui.Joint joint in data.Joints)
                    {
                        if (joint.ID == JointID.HandLeft)
                        {
                            leftHand = ToVector3(joint.Position);
                            Console.WriteLine("Found Left Hand\n");
                        }
                        else if (joint.ID == JointID.HandRight)
                        {
                            rightHand = ToVector3(joint.Position);
                            Console.WriteLine("Found Right Hand\n");
                        }
                        else if (joint.ID == JointID.Head)
                        {
                            head = ToVector3(joint.Position);
                            Console.WriteLine("Found Head");
                        }
                    }
                }
            }
        }

        private Vector3 ToVector3(Vector vector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(handCursor, new Rectangle(Convert.ToInt32(leftHand.X), Convert.ToInt32(leftHand.Y), 50, 60), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(handCursor, new Rectangle(Convert.ToInt32(rightHand.X), Convert.ToInt32(rightHand.Y), 50, 60), Color.Black);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
