using Microsoft.Research.Kinect.Nui;
using Microsoft.Xna.Framework;

namespace GameStateManagement
{
    public static class Tools
    {
        public static Vector2 ToVector2(this Vector vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
