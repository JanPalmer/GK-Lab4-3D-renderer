using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace PRO4_lab
{
    public class Material
    {
        public colorvalue ka, kd, ks; // ambient color, diffuse color, specular color
        public float ns; // specular highlight component
        public float d; // dissolve - determines how much the material blends into the background
        public Brush brush;
    }
}
