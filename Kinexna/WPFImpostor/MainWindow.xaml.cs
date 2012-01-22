using System.Windows;
using Kinexna;
using Microsoft.Research.Kinect.Nui;

namespace WPFImpostor
{
    public partial class MainWindow
    {
        Game1 game;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            Runtime kinectRuntime = new Runtime();
            
            kinectRuntime.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking);
            kinectRuntime.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);

            kinectRuntime.SkeletonFrameReady += kinectRuntime_SkeletonFrameReady;
            kinectRuntime.DepthFrameReady += kinectRuntime_DepthFrameReady;

            kinectRuntime.SkeletonEngine.TransformSmooth = true;

            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.75f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            };
            kinectRuntime.SkeletonEngine.SmoothParameters = parameters;

            using (game = new Game1())
            {
                game.Exiting += game_Exiting;
                game.Run();
            }

            kinectRuntime.Uninitialize();
        }

        void kinectRuntime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            game.OnDepthFrameReady(e);
        }

        void game_Exiting(object sender, System.EventArgs e)
        {
            Close();
        }

        void kinectRuntime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            game.OnSkeletonFrameReady(e);
        }
    }
}
