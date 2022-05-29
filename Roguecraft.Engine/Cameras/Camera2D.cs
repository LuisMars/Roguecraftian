using Microsoft.Xna.Framework;

namespace Roguecraft.Engine.Cameras
{
    public class Camera2D
    {
        private Matrix _camRotationMatrix = Matrix.Identity;
        private Matrix _camScaleMatrix = Matrix.Identity;
        private Vector3 _camScaleVector = Vector3.Zero;
        private Matrix _camTranslationMatrix = Matrix.Identity;
        private Vector3 _camTranslationVector = Vector3.Zero;
        private Vector2 _position;
        private Matrix _resTranslationMatrix = Matrix.Identity;
        private Vector3 _resTranslationVector = Vector3.Zero;
        private float _rotation;
        private Matrix _transform = Matrix.Identity;

        private float _zoom;

        public Camera2D(int realWidth, int realHeight)
        {
            ScreenWidth = realWidth;
            ScreenHeight = realHeight;

            _zoom = 0.5f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
            MinZoom = 0.1f;
            MaxZoom = 999f;
        }

        public float MaxZoom { get; set; }
        public float MinZoom { get; set; }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public float Zoom
        {
            get => _zoom;
            set => _zoom = Math.Clamp(value, MinZoom, MaxZoom);
        }

        private int ScreenHeight { get; set; }

        private int ScreenWidth { get; set; }

        public Matrix GetViewTransformationMatrix()
        {
            _camTranslationVector.X = -_position.X;
            _camTranslationVector.Y = -_position.Y;

            Matrix.CreateTranslation(ref _camTranslationVector, out _camTranslationMatrix);
            Matrix.CreateRotationZ(_rotation, out _camRotationMatrix);

            _camScaleVector.X = _zoom;
            _camScaleVector.Y = _zoom;
            _camScaleVector.Z = 1;

            Matrix.CreateScale(ref _camScaleVector, out _camScaleMatrix);

            _resTranslationVector.X = ScreenWidth * 0.5f;
            _resTranslationVector.Y = ScreenHeight * 0.5f;
            _resTranslationVector.Z = 0;

            Matrix.CreateTranslation(ref _resTranslationVector, out _resTranslationMatrix);

            _transform = _camTranslationMatrix *
                         _camRotationMatrix *
                         _camScaleMatrix *
                         _resTranslationMatrix;

            return _transform;
        }

        public bool IsInView(Vector2 coord, float expand = 1)
        {
            var display = ToDisplay(coord);

            return display.X > -ScreenWidth * (expand - 1) &&
                display.X < ScreenWidth * expand &&
                display.Y > -ScreenWidth * (expand - 1) &&
                display.Y < ScreenHeight * expand;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public Vector2 ToDisplay(Vector2 coord)
        {
            return Vector2.Transform(coord, GetViewTransformationMatrix());
        }

        public void Update(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
        }
    }
}