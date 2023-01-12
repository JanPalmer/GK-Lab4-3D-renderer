using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRO4_lab
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Matrix4x4 P, T, Ry;
        float angle = 0.01f;
        Mesh mesh;
        Model model;
        Camera camera;
        Renderer renderer;

        public Form1()
        {
            InitializeComponent();

            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            bitmap = new Bitmap(width, height);

            pictureBoxMain.BackgroundImage = bitmap;
            Graphics g = Graphics.FromImage(pictureBoxMain.BackgroundImage);
            g.Clear(Color.White);

            //string pathOBJ = ".\\assets\\pooltable.obj";
            //string pathMTL = ".\\assets\\pooltable.mtl";
            //string pathOBJ = ".\\assets\\amogus.obj";
            //string pathMTL = ".\\assets\\amogus.mtl";
            //string pathOBJ = ".\\assets\\nose.obj";
            string pathOBJ = ".\\assets\\cube.obj";

            //mesh = Mesh.FromOBJ(pathOBJ, pathMTL);
            mesh = Mesh.FromOBJ(pathOBJ);
            model = new Model(mesh);
            camera = new Camera(new Vector4(3, 0, 0.1f, 0), new Vector4(0, 0, 0, 0), pictureBoxMain.Size);
            renderer = new Renderer();
            renderer.addCamera(camera);
            renderer.addObject(model, new Vector4(0, 0, 0, 0));

            //DrawCube();
            timer1.Start();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            camera.viewportSize = pictureBoxMain.Size;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(pictureBoxMain.BackgroundImage);
            g.Clear(Color.White);
            model.RotateByOY(angle);
            //angle += 0.01f;
            renderer.renderScene(g);
            pictureBoxMain.Refresh();
        }

        private void DrawCube()
        {
            
            Matrix4x4 transpose = T * Ry;
            transpose = P * transpose;

            Vector4[] triangleTransposed = new Vector4[3];
            Point[] triangleToDisplay = new Point[3];

            Graphics g = Graphics.FromImage(pictureBoxMain.BackgroundImage);

            foreach(Face f in mesh.faces)
            {
                for(int i = 0; i < 3; i++)
                {
                    triangleTransposed[i] = VectorUtils.MultiplyMatrix4x4andVector4(transpose, f.GetVertex(i));
                    triangleToDisplay[i] = new Point(
                    (int)(triangleTransposed[i].X / triangleTransposed[i].W),
                    (int)(triangleTransposed[i].Y / triangleTransposed[i].W)
                    );
                }

                g.DrawLines(Pens.Black, triangleToDisplay);
            }

            angle += 0.1f;
            pictureBoxMain.Refresh();
        }
    }
}
