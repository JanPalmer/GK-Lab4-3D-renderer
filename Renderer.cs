using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PRO4_lab
{
    class Renderer
    {
        private List<Camera> cameras;
        private int currentCamera = 0;
        private List<Model> models;
        private Bitmap output;

        public Renderer()
        {
            cameras = new List<Camera>();
            models = new List<Model>();
        }

        // getters, setters
        public Camera getCurrentCamera()
        {
            return cameras[currentCamera];
        }
        public void addCamera(Camera camera)
        {
            cameras.Add(camera);
        }
        public void changeCamera(int i)
        {
            if (i < 0) i = 0;
            if (i > cameras.Count) i = cameras.Count - 1;
            currentCamera = i;
        }
        public void addObject(Model model, Vector4 position)
        {
            models.Add(model);
            model.MoveByVector(position);
        }
        public void setBitmap(Bitmap bitmap)
        {
            output = bitmap;
        }
        public Bitmap getBitmap()
        {
            return output;
        }

        // rendering
        public void renderScene(Graphics g)
        {
            Camera currentCamera = getCurrentCamera();
            Matrix4x4 view = currentCamera.getViewMatrix();
            Matrix4x4 proj = currentCamera.getProjectionMatrix();

            Matrix4x4 transpose = proj * view;

            Parallel.ForEach(models, (model) =>
            {
                model.meshTranslated = new Mesh(model.mesh);
                Parallel.For(0, model.mesh.vertices.Count, (index) =>
                {
                    Vector4 v = model.mesh.vertices[index];
                    model.meshTranslated.vertices[index] = VectorUtils.MultiplyMatrix4x4andVector4(transpose, v);
                });

                //Parallel.For(0, model.mesh.normalVectors.Count, (index) =>
                //{
                //    Vector4 nv = model.mesh.normalVectors[index];
                //    model.meshTranslated.normalVectors[index] = VectorUtils.MultiplyMatrix4x4andVector4(transpose, nv);
                //});
            });

            //int sum = 0;
            foreach (Model model in models)
            {
                foreach (Face f in model.meshTranslated.faces)
                {
                    //sum++;

                    //Vector4 p0p1 = f.GetVertex(1) - f.GetVertex(0);
                    //Vector4 p0p2 = f.GetVertex(2) - f.GetVertex(0);
                    //Vector4 cross = VectorUtils.CrossProduct(p0p1, p0p2);
                    //cross.W = 0;
                    //cross = Vector4.Normalize(cross);
                    //if (Vector4.Dot(currentCamera.position, cross) > 0) continue;
                    Vector4 nvAvg = new Vector4();
                    int count = 0;
                    for (int i = 0; i < f.vertices.Count; i++)
                    {
                        nvAvg += f.GetNormal(i);
                        count++;
                    }
                    nvAvg /= count;
                    if (Vector4.Dot(currentCamera.position, nvAvg) <= 0.6f) continue;

                    Point[] triangleToDisplay = new Point[f.vertices.Count + 1];
                    for (int i = 0; i < f.vertices.Count; i++)
                    {
                        Vector4 v = f.GetVertex(i);
                        triangleToDisplay[i] = new Point((int)(v.X / v.W), (int)(v.Y / v.W));
                    }
                    triangleToDisplay[f.vertices.Count] = triangleToDisplay[0];
                    g.DrawLines(Pens.Black, triangleToDisplay);
                    //g.FillPolygon(model.mesh.materials[f.materialName].brush, triangleToDisplay);
                }
            }
            //Form1.ActiveForm.Text = $"triangles: {sum}";
        }
    }
}
