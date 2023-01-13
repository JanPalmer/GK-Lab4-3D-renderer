using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PRO4_lab
{
    public class Model
    {
        public Mesh meshTranslated;
        public Mesh mesh;

        public Vector4 position;
        public float rotationX, rotationY, rotationZ;

        public Model(Mesh _mesh)
        {            
            mesh = _mesh;
            meshTranslated = new Mesh(_mesh);
        }

        private void _MultiplyEveryMeshVertexByMatrix(Matrix4x4 matrix)
        {
            Parallel.For(0, mesh.vertices.Count,
                ctr => { 
                    Vector4 v = mesh.vertices[ctr];
                    mesh.vertices[ctr] = VectorUtils.MultiplyMatrix4x4andVector4(matrix, v);                
                });
        }

        private void _MultiplyEveryMeshNormalByMatrix(Matrix4x4 matrix)
        {
            Parallel.For(0, mesh.normalVectors.Count,
                ctr => {
                    Vector4 v = mesh.normalVectors[ctr];
                    mesh.normalVectors[ctr] = VectorUtils.MultiplyMatrix4x4andVector4(matrix, v);
                });
        }

        public void MoveByVector(Vector4 shift)
        {
            position += shift;
            Matrix4x4 shiftMatrix = new Matrix4x4(
                1, 0, 0, shift.X,
                0, 1, 0, shift.Y,
                0, 0, 1, shift.Z,
                0, 0, 0, 1);
            _MultiplyEveryMeshVertexByMatrix(shiftMatrix);
            _MultiplyEveryMeshNormalByMatrix(shiftMatrix);
        }

        public void RotateByOX(float theta)
        {
            rotationX += theta;
            float sin = (float)Math.Sin(theta);
            float cos = (float)Math.Cos(theta);

            Matrix4x4 rotationMatrix = new Matrix4x4(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1
                );

            _MultiplyEveryMeshVertexByMatrix(rotationMatrix);
            _MultiplyEveryMeshNormalByMatrix(rotationMatrix);
        }

        public void RotateByOY(float theta)
        {
            rotationY += theta;
            float sin = (float)Math.Sin(theta);
            float cos = (float)Math.Cos(theta);

            Matrix4x4 rotationMatrix = new Matrix4x4(
                cos, 0, -sin, 0,
                0, 1, 0, 0,
                sin, 0, cos, 0,
                0, 0, 0, 1
                );

            _MultiplyEveryMeshVertexByMatrix(rotationMatrix);
            _MultiplyEveryMeshNormalByMatrix(rotationMatrix);
        }

        public void RotateByOZ(float theta)
        {
            rotationZ += theta;
            float sin = (float)Math.Sin(theta);
            float cos = (float)Math.Cos(theta);

            Matrix4x4 rotationMatrix = new Matrix4x4(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
                );

            _MultiplyEveryMeshVertexByMatrix(rotationMatrix);
            _MultiplyEveryMeshNormalByMatrix(rotationMatrix);
        }
    }
}
