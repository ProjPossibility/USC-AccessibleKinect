using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Research.Kinect.Nui;

namespace WindowsGame1
{
    public static class Tools
    {
        public static Microsoft.Xna.Framework.Vector3 ToVector3(this Microsoft.Research.Kinect.Nui.Vector vector)
        {
            return new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
