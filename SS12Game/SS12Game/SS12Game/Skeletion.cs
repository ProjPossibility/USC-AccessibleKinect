using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using Microsoft.Xna.Framework;

namespace SS12Game
{
    class Skeletion
    {
        Runtime kinectSensor = new Runtime();

        Vector2 position = new Vector2(0.0f);
        Vector2 resolution = new Vector2(0.0f);
        float scale = 1.0f;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        public void Initialize(float scale, int width, int height)
        {
            resolution = new Vector2((float)width, (float)height);
            this.scale = scale;

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

        void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame allSkeletons = e.SkeletonFrame;
            SkeletonData playerSkeleton = (from s in allSkeletons.Skeletons where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
            Joint rightHandJoint = playerSkeleton.Joints[JointID.HandRight];

            try
            {
                position = new Vector2((((0.5f * rightHandJoint.Position.X) + 0.5f) * (resolution.X)), (((-0.5f * rightHandJoint.Position.Y) + 0.5f) * (resolution.Y)));
            }
            catch
            {
            }

        }

    }
}
