using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Research.Kinect.Nui;
using Matrix = Microsoft.Xna.Framework.Matrix;


namespace GameStateManagement
{
    class Mover
    {
        Runtime kinectSensor;
        float player1Theta = 0;
        float player2Theta = 0;
        GameStateManagementGame thisGameIsAwesome;
        Vector2 player1LeftHand = Vector2.Zero; Vector2 player1LeftElbow = Vector2.Zero; Vector2 player2RightHand = Vector2.Zero; Vector2 player2RightElbow = Vector2.Zero;


        public Mover(GameStateManagementGame myGame)
        {
            thisGameIsAwesome = myGame;
            kinectSensor = new Runtime();

            try
            {
                kinectSensor.Initialize(RuntimeOptions.UseSkeletalTracking);

                kinectSensor.SkeletonEngine.TransformSmooth = true;
                TransformSmoothParameters p = new TransformSmoothParameters
                {
                    Smoothing = 0.75f,
                    Correction = 0.0f,
                    Prediction = 0.0f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                };


                kinectSensor.SkeletonEngine.SmoothParameters = p;
               }
            catch (Exception ex)
            {
            }
        }

        public void CreateEvent()
        {
            kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);
        }

        public void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Console.WriteLine("Hey, listen");
            SkeletonFrame skeletonFrame = e.SkeletonFrame;
            Boolean player1 = true;
            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (data.TrackingState == SkeletonTrackingState.Tracked)
                    Console.WriteLine("Im watching you");
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

                        player1Theta = (float)Math.Atan(((player1LeftHand.Y - player1LeftElbow.Y) / (player1LeftHand.X - player1LeftElbow.X)));
                        player1Theta = (player1Theta * 180) / (float)Math.PI;
                        Console.WriteLine(player1Theta + "Is my angle");
                        thisGameIsAwesome.screenManager.input.myArmAngle = player1Theta;

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
                }
                //player1 = !player1;
                return;
            }

        }
    }
}
