using Microsoft.Research.Kinect.Nui;
using Microsoft.Xna.Framework;

namespace Kinexna
{
    public static class Tools
    {
        public static Vector3 ToVector3(this Vector vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
