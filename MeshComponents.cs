using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.Numerics;

namespace PRO4_lab
{
    public class Face
    {
        // v - index of a Vertex in its Face's parent (Mesh), nv - index of a Normal Vector in its Face's parent (Mesh)
        public List<(int v, int nv)> vertices;
        public Mesh parent;
        public string materialName;

        public Face()
        {
            vertices = new List<(int, int)>();
        }

        // Getter for a Vertex Object which the Face references
        public Vector4 GetVertex(int i)
        {
            return parent.vertices[vertices[i].v];
        }
        // Getter for a NormalVector Object which the Face references
        public Vector4 GetNormal(int i)
        {
            return parent.normalVectors[vertices[i].nv];
        }
        public Material GetMaterial()
        {
            return parent.materials[materialName];
        }
        public void AddVertex(int v, int nv)
        {
            vertices.Add((v, nv));
        }
    }

    public class Mesh
    {
        public List<Vector4> vertices;
        public List<Vector4> normalVectors;
        public List<Face> faces;
        public Dictionary<string, Material> materials;

        public Mesh()
        {
            vertices = new List<Vector4>();
            normalVectors = new List<Vector4>();
            faces = new List<Face>();
        }

        public Mesh(Mesh mesh)
        {
            vertices = new List<Vector4>(mesh.vertices);
            normalVectors = new List<Vector4>(mesh.normalVectors);
            faces = new List<Face>();
            foreach(Face f in mesh.faces)
            {
                Face newf = new Face();
                newf.vertices = new List<(int v, int nv)>(f.vertices);
                newf.parent = this;
                newf.materialName = new string(f.materialName);
                faces.Add(newf);
            }

            if(mesh.materials != null)
            {
                materials = new Dictionary<string, Material>(mesh.materials);
            }
            else
            {
                materials = null;
            }

        }

        public static Mesh FromOBJ(string pathOBJ, string pathMTL = "")
        {
            if (!File.Exists(pathOBJ)) return null;

            Dictionary<string, Material> materials = _ReadMTLfile(pathMTL);

            FileStream fsOBJ = File.OpenRead(pathOBJ);
            StreamReader readerOBJ = new StreamReader(fsOBJ);

            Mesh mesh = new Mesh();
            mesh.materials = materials;

            string line;
            string[] div;
            CultureInfo ci = CultureInfo.InvariantCulture;
            string currentMaterial = null;

            while ((line = readerOBJ.ReadLine()) != null)
            {
                div = line.Split(' ');

                switch (div[0])
                {
                    case "v":
                        mesh.vertices.Add(new Vector4(float.Parse(div[1], ci), float.Parse(div[2], ci), float.Parse(div[3], ci), 1));
                        break;
                    case "vn":
                        mesh.normalVectors.Add(new Vector4(float.Parse(div[1], ci), float.Parse(div[2], ci), float.Parse(div[3], ci), 0));
                        break;
                    case "f":
                        Face tmpFace = new Face();
                        tmpFace.parent = mesh;

                        int vert, norm;
                        string[] indexString;
                        for (int i = 1; i < div.Length; i++)
                        {
                            indexString = div[i].Split('/');
                            switch (indexString.Length)
                            {                                
                                case 3:
                                    vert = int.Parse(indexString[0], ci) - 1;
                                    norm = int.Parse(indexString[2], ci) - 1;
                                    tmpFace.AddVertex(vert, norm);
                                    break;
                                default:
                                    break;
                            }
                        }
                        if(materials != null
                            && currentMaterial != null
                            && materials.ContainsKey(currentMaterial))
                        {
                            tmpFace.materialName = currentMaterial;
                        }
                        mesh.faces.Add(tmpFace);
                        break;
                    case "usemtl":
                        currentMaterial = div[1];
                        break;
                    default:
                        break;
                }
            }
            return mesh;
        }

        private static Dictionary<string, Material> _ReadMTLfile(string pathMTL)
        {
            if (!File.Exists(pathMTL)) return null;

            FileStream fsMTL = File.OpenRead(pathMTL);
            StreamReader readerMTL = new StreamReader(fsMTL);

            Dictionary<string, Material> materials = new Dictionary<string, Material>();
            string line;
            string[] div;
            CultureInfo ci = CultureInfo.InvariantCulture;
            bool readmaterial = false;
            Material tmpMaterial = null;

            while ((line = readerMTL.ReadLine()) != null)
            {
                div = line.Split(' ');
                if (div.Length <= 1)
                {
                    readmaterial = false;
                    continue;
                }

                switch (div[0])
                {
                    case "newmtl":
                        readmaterial = true;
                        tmpMaterial = new Material();
                        materials.Add(div[1], tmpMaterial);
                        break;
                    case "Ka":
                        if (!readmaterial) break;
                        if (div.Length < 4) throw new Exception("Model - FromOBJ: bad .mtl file");
                        tmpMaterial.ka = new colorvalue(
                            float.Parse(div[1], ci),
                            float.Parse(div[2], ci),
                            float.Parse(div[3], ci));
                        break;
                    case "Kd":
                        if (!readmaterial) break;
                        if (div.Length < 4) throw new Exception("Model - FromOBJ: bad .mtl file");
                        tmpMaterial.kd = new colorvalue(
                            float.Parse(div[1], ci),
                            float.Parse(div[2], ci),
                            float.Parse(div[3], ci));
                        tmpMaterial.brush = new SolidBrush(colorvalue.ToColor(tmpMaterial.kd.R, tmpMaterial.kd.G, tmpMaterial.kd.B));
                        break;
                    case "Ks":
                        if (!readmaterial) break;
                        if (div.Length < 4) throw new Exception("Model - FromOBJ: bad .mtl file");
                        tmpMaterial.ks = new colorvalue(
                            float.Parse(div[1], ci),
                            float.Parse(div[2], ci),
                            float.Parse(div[3], ci));
                        break;
                    case "Ns":
                        if (!readmaterial) break;
                        tmpMaterial.ns = float.Parse(div[1], ci);
                        break;
                    case "d":
                        if (!readmaterial) break;
                        tmpMaterial.d = float.Parse(div[1], ci);
                        break;
                    default:
                        break;
                }
            }
            return materials;
        }
    }


}
