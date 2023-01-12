using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace PRO4_lab
{
    public class colorvalue
    {
        private float r, b, g;
        public float R { get { return r; } }
        public float G { get { return g; } }
        public float B { get { return b; } }
        private Color color;
        public Color Color { get { return color; } }

        public colorvalue(float _r, float _g, float _b)
        {
            r = _r;
            g = _g;
            b = _b;
            color = ToColor();
        }
        public colorvalue()
        {
            r = 1; g = 1; b = 1;
            color = ToColor();
        }
        public colorvalue(Color _color)
        {
            SetColor(_color);
        }

        public void SetColor(float _r, float _g, float _b)
        {
            r = _r; g = _g; b = _b;
            color = ToColor();
        }
        public void SetColor(Color _color)
        {
            r = (float)_color.R / 255.0f;
            g = (float)_color.G / 255.0f;
            b = (float)_color.B / 255.0f;
            color = _color;
        }

        public Color ToColor()
        {
            float rf, gf, bf;
            rf = 255.999f * r;
            gf = 255.999f * g;
            bf = 255.999f * b;

            if (rf < 0) rf = 0;
            if (gf < 0) gf = 0;
            if (bf < 0) bf = 0;

            int finalR, finalG, finalB;
            finalR = ((rf > 255) ? 255 : Convert.ToInt32(rf));
            finalG = ((gf > 255) ? 255 : Convert.ToInt32(gf));
            finalB = ((bf > 255) ? 255 : Convert.ToInt32(bf));

            return Color.FromArgb(finalR, finalG, finalB);
        }

        // Static function
        static public Color ToColor(float R, float G, float B)
        {
            colorvalue color = new colorvalue(R, G, B);
            return color.ToColor();
        }
        static public Vector4 ConvertColorToVector(Color col)
        {
            colorvalue colorval = new colorvalue(col);
            return new Vector4(colorval.r, colorval.g, colorval.b, 0);
        }
    }
}
