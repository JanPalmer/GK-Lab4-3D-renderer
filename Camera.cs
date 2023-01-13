using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace PRO4_lab
{
    class Camera
    {
        static public Vector4 globalUP = new Vector4(0, 1, 0, 1);

        public Vector4 position;
        public Vector4 direction;
        public Vector4 right;
        public Vector4 up;
        public Size viewportSize;
        public float fov;

        public Camera(Vector4 _position, Vector4 _pointToObserve, Size _viewportSize, float _fieldOfView = (float)Math.PI / 4)
        {
            position = _position;
            lookAt(_pointToObserve);
            viewportSize = _viewportSize;
            fov = _fieldOfView;
        }

        public void lookAt(Vector4 point)
        {
            if (position == null) throw new Exception("Camera - lookAt: position not set");
            direction = Vector4.Normalize(point - position);
            right = VectorUtils.CrossProduct(globalUP, direction);
            right = Vector4.Normalize(right);
            up = VectorUtils.CrossProduct(right, direction);
            up = Vector4.Normalize(up);
            if (Form1.ActiveForm == null) return;
            Form1.ActiveForm.Text = $"Camera dir: ${direction.X}, ${direction.Y}, ${direction.Z}, ${direction.W}";
        }

        public Matrix4x4 getViewMatrix()
        {
            Matrix4x4 leftside = new Matrix4x4(
                right.X, right.Y, right.Z, 0,
                up.X, up.Y, up.Z, 0,
                direction.X, direction.Y, direction.Z, 0,
                0, 0, 0, 1
                );

            Matrix4x4 rightside = new Matrix4x4(
                1, 0, 0, -position.X,
                0, 1, 0, -position.Y,
                0, 0, 1, -position.Z,
                0, 0, 0, 1
                );

            return leftside * rightside;
        }

        public Matrix4x4 getProjectionMatrix()
        {
                float cx = viewportSize.Width / 2;
                float cy = viewportSize.Height / 2;
                float s = cy / (float)Math.Tan(fov);

                return new Matrix4x4(
                        s, 0, cx, 0,
                        0, s, cy, 0,
                        0, 0, 0, 1,
                        0, 0, 1, 0
                    );
        }

        public void rotateLeft(float speed)
        {
            Vector4 rotationPoint = (-right * speed + direction) + position;
            lookAt(rotationPoint);
        }
        public void rotateRight(float speed)
        {
            Vector4 rotationPoint = (right * speed + direction) + position;
            lookAt(rotationPoint);
        }
        public void moveForward(float speed)
        {
            position += direction * speed;
            lookAt(position + direction);
        }
        public void moveBackward(float speed)
        {
            position += -direction * speed;
            lookAt(position + direction);
        }
        public void moveLeft(float speed)
        {
            position += -right * speed;
            lookAt(position + direction);
        }
        public void moveRight(float speed)
        {
            position += right * speed;
            lookAt(position + direction);
        }
    }
}
