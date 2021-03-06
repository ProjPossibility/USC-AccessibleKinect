using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Research.Kinect.Nui;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Kinexna
{
    public class Game1 : Game
    {
        enum Mode
        {
            Control,
            UI,
            Waiting
        }

        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model ship;
        SpriteFont font;
        Texture2D depthTexture;
        readonly byte[] depthFrame32 = new byte[320 * 240 * 4];

        Mode currentMode = Mode.Waiting;

        bool displayDepthTexture;

        float oldHorizontalDistance;
        float rightHandReference;
        float player1Theta = 0;
        float player2Theta = 0;

        float rotationAngle;
        float zoom = 1.0f;
        Vector2 player1LeftHand = Vector2.Zero; Vector2 player1LeftElbow = Vector2.Zero; Vector2 player2RightHand = Vector2.Zero; Vector2 player2RightElbow = Vector2.Zero;

        readonly List<float> horizontalDistances = new List<float>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
                           {
                               PreferredBackBufferWidth = 1024,
                               PreferredBackBufferHeight = 768
                           };
            Content.RootDirectory = "Content";
        }

        void ConvertDepthFrame(byte[] depthFrame16)
        {
            for (int i16 = 0, i32 = 0; i16 < depthFrame16.Length && i32 < depthFrame32.Length; i16 += 2, i32 += 4)
            {
                int player = depthFrame16[i16] & 0x07;
                int realDepth = (depthFrame16[i16 + 1] << 5) | (depthFrame16[i16] >> 3);
                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                byte intensity = (byte)(255 - (255 * realDepth / 0x0fff));

                depthFrame32[i32] = 0;
                depthFrame32[i32 + 1] = 0;
                depthFrame32[i32 + 2] = 0;

                switch (player)
                {
                    case 0: // no one
                        depthFrame32[i32] = (byte)(intensity / 2);
                        depthFrame32[i32 + 1] = (byte)(intensity / 2);
                        depthFrame32[i32 + 2] = (byte)(intensity / 2);
                        break;
                    default:
                        depthFrame32[i32] = intensity;
                        break;
                }
            }
        }

        public void OnDepthFrameReady(ImageFrameReadyEventArgs e)
        {
            lock (this)
            {
                if (GraphicsDevice == null)
                    return;
                if (depthTexture == null)
                {
                    depthTexture = new Texture2D(GraphicsDevice, e.ImageFrame.Image.Width, e.ImageFrame.Image.Height);
                }
                PlanarImage Image = e.ImageFrame.Image;
                ConvertDepthFrame(Image.Bits);

                GraphicsDevice.Textures[0] = null;
                depthTexture.SetData(depthFrame32);
            }
        }

        public void OnSkeletonFrameReady(SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;
            Boolean player1 = true;
            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (data.TrackingState == SkeletonTrackingState.Tracked)
                {
                    if (player1)
                    {
                        
                        foreach (Joint joint in data.Joints)
                        {
                            if (joint.ID == JointID.HandLeft)
                            {
                                player1LeftHand = joint.Position.ToVector2();
                            }
                            else if (joint.ID == JointID.ElbowLeft)
                            {
                                player1LeftElbow = joint.Position.ToVector2();
                            }
                        }

                        player1Theta = (float)Math.Atan( ( (player1LeftHand.Y - player1LeftElbow.Y) / (player1LeftHand.X - player1LeftElbow.X) ));

                    }
                    else
                    {
                        
                        foreach (Joint joint in data.Joints)
                        {
                            if (joint.ID == JointID.HandRight)
                            {
                                player2RightHand = joint.Position.ToVector2();
                            }
                            else if (joint.ID == JointID.ElbowRight)
                            {
                                player2RightElbow = joint.Position.ToVector2();
                            }
                        }
                        player2Theta = (float)Math.Atan(((player2RightHand.Y - player2RightElbow.Y) / (player2RightHand.X - player2RightElbow.X)));
                    }
                    /*Vector3 diffVector = (leftHand - rightHand);
                    float horizontalDistance = Math.Abs(diffVector.X);
                    float verticalDistance = Math.Abs(diffVector.Y);
                    float depthDistance = diffVector.Z;

                    Vector3 headToRightHand = (head - rightHand);*/


                    /*switch (currentMode)
                    {
                        case Mode.Control:
                            // Rotate
                            horizontalDistances.Add(horizontalDistance - oldHorizontalDistance);

                            // Smoothing a bit...
                            if (horizontalDistances.Count > 20)
                            {
                                horizontalDistances.RemoveAt(0);
                            }

                            if (horizontalDistances.Count == 20)
                            {
                                float rotateAvg = horizontalDistances.Average();
                                rotationAngle += rotateAvg * 5.0f;
                            }

                            // Zoom
                            if (Math.Abs(depthDistance) > 0.3f)
                            {

                                zoom += depthDistance * 0.1f;

                                if (zoom > 1.5f)
                                    zoom = 1.5f;
                                else if (zoom < 0.2f)
                                    zoom = 0.2f;
                            }

                            if (verticalDistance > 0.5f)
                            {
                                currentMode = Mode.Waiting;
                            }
                            break;
                        case Mode.UI:
                            if (Math.Abs(headToRightHand.Y) > 0.4f)
                            {
                                currentMode = Mode.Waiting;
                            }

                            if (Math.Abs(rightHand.X - rightHandReference) > 0.3f)
                            {
                                rightHandReference = rightHand.X;
                                displayDepthTexture = !displayDepthTexture;
                            }
                            break;
                        case Mode.Waiting:
                            if (horizontalDistance < 0.15f)
                            {
                                currentMode = Mode.Control;
                                horizontalDistances.Clear();
                                oldHorizontalDistance = float.MaxValue;
                            }

                            if (Math.Abs(headToRightHand.Y) < 0.2f)
                            {
                                currentMode = Mode.UI;
                                rightHandReference = rightHand.X;
                            }
                            break;
                    }


                    oldHorizontalDistance = horizontalDistance;*/
                }
               player1 = !player1;
            }
           return;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //ship = Content.Load<Model>("Ship");
            font = Content.Load<SpriteFont>("Segoe");
        }

        protected override void Draw(GameTime gameTime)
        {
            /*lock (this)
            {
                // Standard render
                GraphicsDevice.Clear(Color.Gray);


                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                ship.Draw(Matrix.CreateScale(zoom) * Matrix.CreateRotationY(rotationAngle),
                          Matrix.CreateLookAt(new Vector3(0, 0, 2000), Vector3.Zero, Vector3.Up),
                          Matrix.CreatePerspectiveFieldOfView(0.8f, GraphicsDevice.Viewport.AspectRatio, 0.1f,
                                                              10000.0f));


                // Kinect textures
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

                if (depthTexture != null && displayDepthTexture)
                {
                    spriteBatch.Draw(depthTexture,
                                     new Rectangle(GraphicsDevice.Viewport.Width - 600, 0, 600, 400),
                                     Color.White);
                }
                spriteBatch.End();

                // Status text
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                switch (currentMode)
                {
                    case Mode.Control:
                        spriteBatch.DrawString(font, string.Format("Hands distance : {0:0.00} / Zoom factor : {1:0.00}", oldHorizontalDistance, zoom), new Vector2(0, GraphicsDevice.Viewport.Height - 120), Color.White);
                        spriteBatch.DrawString(font, "Spread your hands horizontally to rotate the ship.", new Vector2(0, GraphicsDevice.Viewport.Height - 90), Color.White);
                        spriteBatch.DrawString(font, "Spread vertically to stop controlling the ship.", new Vector2(0, GraphicsDevice.Viewport.Height - 60), Color.White);
                        spriteBatch.DrawString(font, "Spread your hands in depth to zoom the ship.", new Vector2(0, GraphicsDevice.Viewport.Height - 30), Color.White);
                        break;
                    case Mode.UI:
                        break;
                    case Mode.Waiting:
                        spriteBatch.DrawString(font, "User not detected yet :). Please join your hands to start.", new Vector2(0, GraphicsDevice.Viewport.Height - 30), Color.White);
                        break;
                }
                spriteBatch.End();

                base.Draw(gameTime);
            }*/
            try
            {
                //Console.WriteLine(player1LeftHand.X + " Before p1 Hand " + player1LeftHand.Y);
                //Console.WriteLine(player1LeftElbow.X + " Before p1 Elbow " + player1LeftElbow.Y);
                player1LeftHand.X = (player1LeftHand.X + 1) *(GraphicsDevice.Viewport.Width/2);
                player1LeftHand.Y = (-1 * player1LeftHand.Y + 1) * (GraphicsDevice.Viewport.Height/2);
                player1LeftElbow.X = (player1LeftElbow.X + 1) * (GraphicsDevice.Viewport.Width / 2);
                player1LeftElbow.Y = (-1 * player1LeftElbow.Y + 1) * (GraphicsDevice.Viewport.Height / 2);
                //Console.WriteLine(player1LeftHand.X + " After p1 Hand " + player1LeftHand.Y);
                //Console.WriteLine(player1LeftElbow.X + " After p1 Elbow " + player1LeftElbow.Y);

                //Console.WriteLine(player2RightHand.X + " Before p2 Hand " + player2RightHand.Y);
                //Console.WriteLine(player2RightElbow.X + " Before p2 Elbow " + player2RightElbow.Y);
                player2RightHand.X = (player2RightHand.X + 1) * (GraphicsDevice.Viewport.Width / 2);
                player2RightHand.Y = (-1 * player2RightHand.Y + 1) * (GraphicsDevice.Viewport.Height / 2);
                player2RightElbow.X = (player2RightElbow.X + 1) * (GraphicsDevice.Viewport.Width / 2);
                player2RightElbow.Y = (-1 * player2RightElbow.Y + 1) * (GraphicsDevice.Viewport.Height / 2);
                //Console.WriteLine(player2RightHand.X + " After p2 Hand " + player2RightHand.Y);
                //Console.WriteLine(player2RightElbow.X + " After p2 Elbow " + player2RightElbow.Y);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                spriteBatch.DrawString(font, "P1LH", player1LeftHand, Color.Aquamarine);
                spriteBatch.DrawString(font, "P1LE", player1LeftElbow, Color.Aquamarine);
                spriteBatch.DrawString(font, "P2RH", player2RightHand, Color.DarkGoldenrod);
                spriteBatch.DrawString(font, "P2RE", player2RightElbow, Color.DarkGoldenrod);

                spriteBatch.End();
                player1LeftHand.X = (player1LeftHand.X / (GraphicsDevice.Viewport.Width / 2)) - 1;
                player1LeftHand.Y = -1 * ((player1LeftHand.Y / (GraphicsDevice.Viewport.Height / 2)) - 1);
                player1LeftElbow.X = (player1LeftElbow.X / (GraphicsDevice.Viewport.Width / 2)) - 1;
                player1LeftElbow.Y = -1 * ((player1LeftElbow.Y / (GraphicsDevice.Viewport.Height / 2)) - 1);

                player2RightHand.X = (player2RightHand.X / (GraphicsDevice.Viewport.Width / 2)) - 1;
                player2RightHand.Y = -1 * ((player2RightHand.Y / (GraphicsDevice.Viewport.Height / 2)) - 1);
                player2RightElbow.X = (player2RightElbow.X / (GraphicsDevice.Viewport.Width / 2)) - 1;
                player2RightElbow.Y = -1 * ((player2RightElbow.Y / (GraphicsDevice.Viewport.Height / 2)) - 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
